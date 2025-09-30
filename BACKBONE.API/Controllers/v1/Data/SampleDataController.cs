using BACKBONE.Application.Interfaces;
using BACKBONE.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BACKBONE.API.Controllers.v1.Data
{
    /// <summary>
    /// Controller for managing Sample Data operations
    /// </summary>
    [Route("api/v1/data/sample")]
    [ApiController]
    [Authorize]
    public class SampleDataController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public SampleDataController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Gets all sample data records
        /// </summary>
        /// <returns>A list of all sample data records</returns>
        [HttpGet]
        public IActionResult GetAllSampleData()
        {
            Console.WriteLine("Getting all sample data");
            Core.ResponseClasses.EQResponse<List<SAMPLE_DATA>> response = _unitOfWork.SampleDataRepository.GetAllSampleDataAsync();
            if (response.Success == true)
            {
                Console.WriteLine($"Successfully retrieved {response.Data?.ListValue?.Count ?? 0} sample data records");
                return Ok(response);
            }
            Console.WriteLine($"Failed to retrieve sample data: {response.Message}");
            return BadRequest(response);
        }

        /// <summary>
        /// Gets a specific sample data record by ID
        /// </summary>
        /// <param name="id">The ID of the sample data record to retrieve</param>
        /// <returns>The sample data record with the specified ID</returns>
        [HttpGet("{id}")]
        public IActionResult GetSampleDataById(int id)
        {
            Console.WriteLine($"Getting sample data by ID: {id}");
            var response = _unitOfWork.SampleDataRepository.GetSampleDataByIdAsync(id);
            if (response.Success == true)
            {
                Console.WriteLine($"Successfully retrieved sample data by ID: {id}");
                return Ok(response);
            }
            Console.WriteLine($"Failed to retrieve sample data by ID {id}: {response.Message}");
            return BadRequest(response);
        }

        /// <summary>
        /// Creates a new sample data record
        /// </summary>
        /// <param name="sampleData">The sample data record to create</param>
        /// <returns>The created sample data record</returns>
        [HttpPost]
        public async Task<IActionResult> CreateSampleData(SAMPLE_DATA sampleData)
        {
            Console.WriteLine("Creating new sample data");
            var response = await _unitOfWork.SampleDataRepository.CreateSampleDataAsync(sampleData);
            if (response.Success == true)
            {
                Console.WriteLine($"Successfully created sample data with ID: {response.Data?.SingleValue?.ID}");
                return Ok(response);
            }
            Console.WriteLine($"Failed to create sample data: {response.Message}");
            return BadRequest(response);
        }

        /// <summary>
        /// Updates an existing sample data record
        /// </summary>
        /// <param name="id">The ID of the sample data record to update</param>
        /// <param name="sampleData">The updated sample data record</param>
        /// <returns>The updated sample data record</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSampleData(int id, SAMPLE_DATA sampleData)
        {
            Console.WriteLine($"Updating sample data with ID: {id}");
            var response = await _unitOfWork.SampleDataRepository.UpdateSampleDataAsync(id, sampleData);
            if (response.Success == true)
            {
                Console.WriteLine($"Successfully updated sample data with ID: {id}");
                return Ok(response);
            }
            Console.WriteLine($"Failed to update sample data with ID {id}: {response.Message}");
            return BadRequest(response);
        }

        /// <summary>
        /// Deletes a sample data record
        /// </summary>
        /// <param name="id">The ID of the sample data record to delete</param>
        /// <returns>Success indicator</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSampleData(int id)
        {
            Console.WriteLine($"Deleting sample data with ID: {id}");
            var response = await _unitOfWork.SampleDataRepository.DeleteSampleDataAsync(id);
            if (response.Success == true)
            {
                Console.WriteLine($"Successfully deleted sample data with ID: {id}");
                return Ok(response);
            }
            Console.WriteLine($"Failed to delete sample data with ID {id}: {response.Message}");
            return BadRequest(response);
        }
    }
}