

namespace BeingSearchApp.Server.Data
{
    public class ExtendedLocationRepository : LocationRepository
    {
        private readonly ICsvImportService _csvImportService;
        private readonly ILogger<ExtendedLocationRepository> _logger;
        private readonly string _csvFilePath = "Data/locations.csv";

        public ExtendedLocationRepository(
            ICsvImportService csvImportService,
            ILogger<ExtendedLocationRepository> logger)
            : base(logger)
        {
            _csvImportService = csvImportService;
            _logger = logger;
        }

        public async Task ImportLocationsFromCsvAsync()
        {
            try
            {
                var locations = await _csvImportService.ImportLocationsFromCsvAsync(_csvFilePath);

                if (locations.Any())
                {
                    // Add or update each location
                    foreach (var location in locations)
                    {
                        await AddLocationAsync(location);
                    }

                    _logger.LogInformation($"Successfully imported {locations.Count} locations from CSV");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error importing locations from CSV");
                throw;
            }
        }
    }
}
