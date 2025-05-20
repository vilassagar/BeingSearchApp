using BeingSearchApp.Server.Data;
using BeingSearchApp.Server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BeingSearchApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private readonly ILocationService _locationService;
        private readonly ILogger<LocationsController> _logger;

        public LocationsController(ILocationService locationService, ILogger<LocationsController> logger)
        {
            _locationService = locationService;
            _logger = logger;
        }
        /// <summary>
        /// Gets all locations available between 10 AM and 1 PM for a specific day
        /// </summary>
        /// <param name="day">Optional day of week (defaults to today)</param>
        /// <returns>A list of available locations</returns>
        [HttpGet("available")]
        [ProducesResponseType(typeof(LocationsResponseWrapper), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAvailableLocations([FromQuery] string day = null)
        {
            try
            {
                DayOfWeek? dayOfWeek = null;

                if (!string.IsNullOrEmpty(day) && Enum.TryParse<DayOfWeek>(day, true, out var parsedDay))
                {
                    dayOfWeek = parsedDay;
                }
                else if (!string.IsNullOrEmpty(day))
                {
                    return BadRequest(new { error = "Invalid day parameter. Valid values: Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday" });
                }

                var result = await _locationService.GetAvailableLocationsBetween10And1Async(dayOfWeek);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving available locations");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An error occurred while processing your request" });
            }
        }

        /// <summary>
        /// Gets a location by its ID
        /// </summary>
        /// <param name="id">Location ID</param>
        /// <returns>The location if found</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Location), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetLocationById(int id)
        {
            try
            {
                var location = await _locationService.GetLocationByIdAsync(id);

                if (location == null)
                {
                    return NotFound(new { error = $"Location with ID {id} not found" });
                }

                return Ok(location);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving location with ID {id}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An error occurred while processing your request" });
            }
        }

        /// <summary>
        /// Adds a new location
        /// </summary>
        /// <param name="location">Location data</param>
        /// <returns>The newly created location</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Location), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddLocation([FromBody] Location location)
        {
            try
            {
                if (location == null)
                {
                    return BadRequest(new { error = "Location data is required" });
                }

                if (string.IsNullOrEmpty(location.Name))
                {
                    return BadRequest(new { error = "Location name is required" });
                }

                if (location.AvailableTimeSlots == null || location.AvailableTimeSlots.Count == 0)
                {
                    return BadRequest(new { error = "At least one time slot is required" });
                }

                var newLocation = await _locationService.AddLocationAsync(location);

                return CreatedAtAction(nameof(GetLocationById), new { id = newLocation.Id }, newLocation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding new location");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An error occurred while processing your request" });
            }
        }
    }
}
