using BeingSearchApp.Server.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BeingSearchApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImportController : ControllerBase
    {
        private readonly ExtendedLocationRepository _repository;
        private readonly ILogger<ImportController> _logger;

        public ImportController(ExtendedLocationRepository repository, ILogger<ImportController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        /// <summary>
        /// Imports locations from the CSV file
        /// </summary>
        /// <returns>Status of the import operation</returns>
        [HttpPost("csv")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ImportLocationsFromCsv()
        {
            try
            {
                await _repository.ImportLocationsFromCsvAsync();
                return Ok(new { message = "Locations successfully imported from CSV file" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error importing locations from CSV");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An error occurred while importing locations from CSV" });
            }
        }
    }
}
