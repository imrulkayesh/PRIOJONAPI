using BACKBONE.Application.Interfaces;
using BACKBONE.Core.Dtos;
using BACKBONE.Core.Models;
using BACKBONE.Core.ResponseClasses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BACKBONE.API.Controllers.v1.Common
{
    [Route("api/v1/data/common")]
    [ApiController]
    [Authorize]
    public class CommonController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CommonController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}
        [HttpGet("user-type")]
        [AllowAnonymous]
        public IActionResult GetAllUserTypeData()
        {
            Console.WriteLine("Getting all user type data");
            Core.ResponseClasses.EQResponse<List<U_USER_TYPE_DTO>> response = _unitOfWork.CommonRepository.GetAllUserTypeDataAsync();
            if (response.Success == true)
            {
                Console.WriteLine($"Successfully retrieved {response.Data?.ListValue?.Count ?? 0} data records");
                return Ok(response);
            }
            Console.WriteLine($"Failed to retrieve user type data: {response.Message}");
            return BadRequest(response);
        }

        [HttpGet("division")]
        [AllowAnonymous]
        public IActionResult GetAllDivitionData()
        {
            Console.WriteLine("Getting all division data");
            Core.ResponseClasses.EQResponse<List<S_DIVISION_DTO>> response = _unitOfWork.CommonRepository.GetAllDivitionDataAsync();
            if (response.Success == true)
            {
                Console.WriteLine($"Successfully retrieved {response.Data?.ListValue?.Count ?? 0} data records");
                return Ok(response);
            }
            Console.WriteLine($"Failed to retrieve division data: {response.Message}");
            return BadRequest(response);
        }
        [HttpGet("district/{id}")]
        [AllowAnonymous]
        public IActionResult GetDistrictDataByDivisionId(string id)
        {
            Console.WriteLine($"Getting data by ID: {id}");
            var response = _unitOfWork.CommonRepository.GetDistrictDataByDivisionIdAsync(id);
            if (response.Success == true)
            {
                Console.WriteLine($"Successfully retrieved data by ID: {id}");
                return Ok(response);
            }
            Console.WriteLine($"Failed to retrieve data by ID {id}: {response.Message}");
            return BadRequest(response);
        }
        [HttpGet("thana/{id}")]
        [AllowAnonymous]
        public IActionResult GetThanaDataByDistrictId(string id)
        {
            Console.WriteLine($"Getting data by ID: {id}");
            var response = _unitOfWork.CommonRepository.GetThanaDataByDistrictIdAsync(id);
            if (response.Success == true)
            {
                Console.WriteLine($"Successfully retrieved data by ID: {id}");
                return Ok(response);
            }
            Console.WriteLine($"Failed to retrieve data by ID {id}: {response.Message}");
            return BadRequest(response);
        }
        // test
        [HttpGet("user-division-data")]
        [AllowAnonymous]
        public async Task<ActionResult<EQResponse2<UserDivisionData>>> GetUserDivisionData()
        {
            // Await the async call to the service
            var response = await _unitOfWork.CommonRepository.GetUserDivisionDataAsync();

            if (response.Success)
                return Ok(response);

            return BadRequest(response);
        }

    }
}
