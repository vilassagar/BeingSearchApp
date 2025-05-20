using BeingSearchApp.Server.Models;

namespace BeingSearchApp.Server.Data
{
    public interface ILocationService
    {
        Task<LocationsResponseWrapper> GetAvailableLocationsBetween10And1Async(DayOfWeek? day = null);
        Task<Location> GetLocationByIdAsync(int id);
        Task<Location> AddLocationAsync(Location location);
    }
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _locationRepository;
        private readonly ILogger<LocationService> _logger;

        public LocationService(ILocationRepository locationRepository, ILogger<LocationService> logger)
        {
            _locationRepository = locationRepository;
            _logger = logger;
        }

        public async Task<LocationsResponseWrapper> GetAvailableLocationsBetween10And1Async(DayOfWeek? day = null)
        {
            try
            {
                var locations = await _locationRepository.GetAllLocationsAsync();
                var filteredDay = day ?? DateTime.Now.DayOfWeek;

                var targetOpenTime = new TimeSpan(10, 0, 0);
                var targetCloseTime = new TimeSpan(13, 0, 0);

                var filteredLocations = locations
                    .Select(location =>
                    {
                        var timeSlot = location.AvailableTimeSlots
                            .FirstOrDefault(ts => ts.DayOfWeek == filteredDay);

                        bool isAvailableBetween10And1 = false;
                        TimeSpan openTime = TimeSpan.Zero;
                        TimeSpan closeTime = TimeSpan.Zero;

                        if (timeSlot != null)
                        {
                            openTime = timeSlot.OpenTime;
                            closeTime = timeSlot.CloseTime;

                            // Check if the location is open during the entire 10AM-1PM period
                            isAvailableBetween10And1 =
                                timeSlot.OpenTime <= targetOpenTime && timeSlot.CloseTime >= targetCloseTime;
                        }

                        return new LocationResponse
                        {
                            Id = location.Id,
                            Name = location.Name,
                            Type = location.Type,
                            Address = location.Address,
                            OpenTime = openTime,
                            CloseTime = closeTime,
                            IsAvailableBetween10And1 = isAvailableBetween10And1
                        };
                    })
                    .ToList();

                var availableLocations = filteredLocations
                    .Where(l => l.IsAvailableBetween10And1)
                    .ToList();

                var response = new LocationsResponseWrapper
                {
                    Locations = availableLocations,
                    TotalCount = availableLocations.Count,
                    Message = availableLocations.Count > 0
                        ? $"Found {availableLocations.Count} locations available between 10 AM and 1 PM on {filteredDay}"
                        : $"No locations available between 10 AM and 1 PM on {filteredDay}"
                };

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting available locations");
                throw;
            }
        }
        public async Task<Location> GetLocationByIdAsync(int id)
        {
            return await _locationRepository.GetLocationByIdAsync(id);
        }

        public async Task<Location> AddLocationAsync(Location location)
        {
            return await _locationRepository.AddLocationAsync(location);
        }
    }
}
