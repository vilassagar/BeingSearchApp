using BeingSearchApp.Server.Models;
using CsvHelper;
using CsvHelper.Configuration;
using System.Formats.Asn1;
using System.Globalization;

namespace BeingSearchApp.Server.Data
{
    public interface ICsvImportService
    {
        Task<List<Location>> ImportLocationsFromCsvAsync(string filePath);
    }
    public class CsvImportService : ICsvImportService
    {
        private readonly ILogger<CsvImportService> _logger;

        public CsvImportService(ILogger<CsvImportService> logger)
        {
            _logger = logger;
        }

        public async Task<List<Location>> ImportLocationsFromCsvAsync(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    _logger.LogError($"CSV file not found at path: {filePath}");
                    return new List<Location>();
                }

                // Read CSV file
                using var reader = new StreamReader(filePath);
                using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture));

                var records = csv.GetRecords<LocationCsvRecord>().ToList();

                if (!records.Any())
                {
                    _logger.LogWarning("No records found in CSV file");
                    return new List<Location>();
                }

                // Group by location ID and convert to Location objects
                var locations = records
                    .GroupBy(r => r.Id)
                    .Select(group => new Location
                    {
                        Id = group.Key,
                        Name = group.First().Name,
                        Type = group.First().Type,
                        Address = group.First().Address,
                        AvailableTimeSlots = group.Select(r => new TimeSlot
                        {
                            DayOfWeek = ParseDayOfWeek(r.DayOfWeek),
                            OpenTime = ParseTimeSpan(r.OpenTime),
                            CloseTime = ParseTimeSpan(r.CloseTime)
                        }).ToList()
                    })
                    .ToList();

                _logger.LogInformation($"Successfully imported {locations.Count} locations from CSV");
                return locations;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error importing locations from CSV file: {filePath}");
                throw;
            }
        }
        private DayOfWeek ParseDayOfWeek(string day)
        {
            if (Enum.TryParse<DayOfWeek>(day, true, out var result))
            {
                return result;
            }

            return DayOfWeek.Monday; // Default value
        }

        private TimeSpan ParseTimeSpan(string time)
        {
            if (TimeSpan.TryParse(time, out var result))
            {
                return result;
            }

            return TimeSpan.Zero; // Default value
        }
    }
}
