using BACKBONE.Application.Interfaces;
using BACKBONE.Application.Interfaces.SecurityInterface;
using BACKBONE.Core.Dtos;
using BACKBONE.Core.Models;
using BACKBONE.Core.ResponseClasses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BACKBONE.API.Controllers.v1.Auth
{
    [Route("api/v1/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly JWT_SETTINGS _jwtSettings;

        public AuthController(
            IUnitOfWork unitOfWork,
            JWT_SETTINGS jwtSettings)
        {
            _unitOfWork = unitOfWork;
            _jwtSettings = jwtSettings;
        }

        [HttpGet("public-settings")]
        [AllowAnonymous]
        public IActionResult GetPublicJwtSettings()
        {
            Console.WriteLine("Public JWT settings requested");
            return Ok(new {
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                ExpiryMinutes = _jwtSettings.ExpiryMinutes,
                KeyLength = _jwtSettings.Key?.Length
            });
        }

        [HttpGet("settings")]
        public IActionResult GetJwtSettings()
        {
            Console.WriteLine("JWT settings requested");
            return Ok(new {
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                ExpiryMinutes = _jwtSettings.ExpiryMinutes
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] LOGIN_DTO loginDto)
        {
            if (loginDto == null || string.IsNullOrEmpty(loginDto.EMP_CODE) || string.IsNullOrEmpty(loginDto.PASSWORD))
            {
                Console.WriteLine("Invalid login data provided");
                return BadRequest("Invalid login data");
            }

            try
            {
                Console.WriteLine($"Login attempt for user: {loginDto.EMP_CODE}");
                
                // Step 1: Find active users using email only through UserRepository
                var user = await _unitOfWork.UserRepository.GetUserByEmailAsync(loginDto.EMP_CODE);

                if (user != null)
                {
                    // Since there's no HRIS system, authenticate directly against the database
                    Console.WriteLine($"Authenticating user: {user.USER_ID}");
                    bool isAuthenticated = (user.PASSWORD == loginDto.PASSWORD);

                    if (isAuthenticated)
                    {
                        Console.WriteLine($"User {user.USER_ID} authenticated successfully");
                        Console.WriteLine($"JWT Settings - Expiry Minutes: {_jwtSettings.ExpiryMinutes}");
                        
                        // Create a minimal user object for JWT generation to avoid exposing sensitive data
                        var jwtUserDto = new JWT_USER_DTO
                        {
                            USER_ID = user.USER_ID,
                            USER_MAIL = user.EMAIL,
                            USER_NAME = user.NAME
                        };

                        // Generate JWT token using the minimal user DTO
                        var jwtToken = _unitOfWork.JwtTokenService.GenerateToken(jwtUserDto);
                        if (jwtToken == null)
                        {
                            Console.WriteLine($"Error generating JWT token for user {user.USER_ID}");
                            return StatusCode(500, "Error generating JWT token");
                        }

                        // Generate refresh token
                        var refreshToken = _unitOfWork.RefreshTokenService.GenerateRefreshTokenAsync();
                        if (refreshToken == null || string.IsNullOrEmpty(refreshToken.ToString()))
                        {
                            Console.WriteLine($"Error generating refresh token for user {user.USER_ID}");
                            return StatusCode(500, "Error generating refresh token");
                        }

                        // Save refresh token to database
                        await _unitOfWork.RefreshTokenService.SaveRefreshTokenAsync(new REFRESH_TOKEN
                        {
                            USER_ID = user.USER_ID, // Use USER_ID directly from U_USER
                            TOKEN = refreshToken.ToString(),
                            EXPIRATION_DATE = DateTime.Now.AddDays(7)
                        });

                        Console.WriteLine($"Tokens generated for user {user.USER_ID}");

                        // Return tokens
                        return Ok(new AUTH_RESPONSE_DTO
                        {
                            TOKEN = jwtToken,
                            REFRESH_TOKEN = refreshToken.ToString()
                        });
                    }
                    else
                    {
                        Console.WriteLine($"Authentication failed for user: {loginDto.EMP_CODE}");
                        return Unauthorized("Invalid credentials");
                    }
                }
                else
                {
                    Console.WriteLine($"User not found: {loginDto.EMP_CODE}");
                    return Unauthorized("Invalid credentials");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during authentication for user: {loginDto.EMP_CODE}: {ex.Message}");
                return StatusCode(500, "Error during authentication: " + ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            Console.WriteLine("Refresh token request received");
            
            if (string.IsNullOrEmpty(refreshToken))
            {
                Console.WriteLine("Refresh token is required");
                return BadRequest("Refresh token is required");
            }

            try
            {
                var isValid = await _unitOfWork.RefreshTokenService.ValidateRefreshTokenAsync(refreshToken);
                if (!isValid)
                {
                    Console.WriteLine("Invalid refresh token provided");
                    return Unauthorized("Invalid refresh token");
                }

                var storedToken = await _unitOfWork.RefreshTokenService.GetRefreshTokenAsync(refreshToken);
                if (storedToken == null)
                {
                    Console.WriteLine("Refresh token not found");
                    return Unauthorized("Refresh token not found");
                }

                // Fetch user details from database through UserRepository
                var user = await _unitOfWork.UserRepository.GetJwtUserByIdAsync(storedToken.USER_ID);

                if (user == null)
                {
                    Console.WriteLine("User not found or inactive for refresh token");
                    return Unauthorized("User not found or inactive");
                }

                // Generate new JWT token
                var newJwtToken = _unitOfWork.JwtTokenService.GenerateToken(user);
                var newRefreshToken = _unitOfWork.RefreshTokenService.GenerateRefreshTokenAsync();

                // Revoke old refresh token and save new one
                await _unitOfWork.RefreshTokenService.RevokeRefreshTokenAsync(refreshToken);
                await _unitOfWork.RefreshTokenService.SaveRefreshTokenAsync(new REFRESH_TOKEN
                {
                    USER_ID = storedToken.USER_ID,
                    TOKEN = newRefreshToken,
                    EXPIRATION_DATE = DateTime.Now.AddDays(7)
                });

                Console.WriteLine($"Tokens refreshed successfully for user {user.USER_ID}");

                // Return new tokens
                return Ok(new AUTH_RESPONSE_DTO 
                { 
                    TOKEN = newJwtToken, 
                    REFRESH_TOKEN = newRefreshToken 
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during token refresh: {ex.Message}");
                return StatusCode(500, "Error during token refresh: " + ex.Message);
            }
        }

        //[HttpPost("registration")]
        //[AllowAnonymous]
        //public async Task<IActionResult> CreateUserData(USER_REGISTRATION_DTO userData)
        //{
        //    Console.WriteLine("Creating new sample data");
        //    var response = await _unitOfWork.UserRepository.CreateUserDataAsync(userData);
        //    if (response.Success == true)
        //    {
        //        Console.WriteLine($"Successfully created user data with ID: {response.Data?.SingleValue?.ID}");
        //        return Ok(response);
        //    }
        //    Console.WriteLine($"Failed to create user data: {response.Message}");
        //    return BadRequest(response);
        //}
    }
}