using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BeingSearchApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthCheckController : ControllerBase
    {
        private readonly ILogger<HealthCheckController> _logger;

        public HealthCheckController(ILogger<HealthCheckController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Checks if the API is running
        /// </summary>
        /// <returns>Health status of the API</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Get()
        {
            _logger.LogInformation("Health check executed at: {time}", DateTime.UtcNow);

            return Ok(new
            {
                status = "Healthy",
                timestamp = DateTime.UtcNow,
                version = GetType().Assembly.GetName().Version.ToString()
            });
        }
    }
}
