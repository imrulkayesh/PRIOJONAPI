using BACKBONE.Application.Interfaces;
using BACKBONE.Core.Models;
using BACKBONE.Core.ResponseClasses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BACKBONE.API.Controllers.v1.User
{
    /// <summary>
    /// Controller for managing user operations
    /// </summary>
    [Route("api/v1/user")]
    [ApiController]
    [AllowAnonymous]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        /// <summary>
        /// Gets user information without authentication
        /// </summary>
        /// <returns>User information</returns>
        [HttpGet("withoutauth")]
        [AllowAnonymous]
        public IActionResult GetUserWithoutAuth()
        {
            Console.WriteLine("Getting user information without authentication");
            
            var response = new EQResponse<object>
            {
                Message = "User information retrieved successfully",
                Success = true,
                Data = new EQResponseData<object>
                {
                    SingleValue = new
                    {
                        UserId = 1,
                        EmpCode = "EMP001",
                        FullName = "John Doe"
                    }
                }
            };

            Console.WriteLine("User information retrieved successfully");
            return Ok(response);
        }
        [HttpPost("registration")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateUserData(U_USER userData)
        {
            Console.WriteLine("Creating new sample data");
            var response = await _unitOfWork.UserRepository.CreateUserDataAsync(userData);
            if (response.Success == true)
            {
                Console.WriteLine($"Successfully created user data with ID: {response.Data?.SingleValue?.ID}");
                return Ok(response);
            }
            Console.WriteLine($"Failed to create user data: {response.Message}");
            return BadRequest(response);
        }
    }
}