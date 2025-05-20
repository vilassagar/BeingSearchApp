using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BeingSearchApp.Server.Models;
using System.Xml;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;


namespace BeingSearchApp.Server.Data
{
    public interface ILocationRepository
    {
        Task<List<Location>> GetAllLocationsAsync();
        Task<Location> GetLocationByIdAsync(int id);
        Task<Location> AddLocationAsync(Location location);
    }

    public class LocationRepository : ILocationRepository
    {
        private readonly ILogger<LocationRepository> _logger;
        private readonly string _dataFilePath = "Data/locations.json";
        private List<Location> _locations;

        public LocationRepository(ILogger<LocationRepository> logger)
        {
            _logger = logger;
            LoadLocationsFromFile();
        }

        private void LoadLocationsFromFile()
        {
            try
            {
                if (File.Exists(_dataFilePath))
                {
                    var json = File.ReadAllText(_dataFilePath);
                    _locations = JsonConvert.DeserializeObject<List<Location>>(json);
                }
                else
                {
                    _logger.LogWarning("Locations data file not found. Using default data.");
                    _locations = GetDefaultLocations();
                    SaveLocationsToFile();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading locations from file");
                _locations = GetDefaultLocations();
            }
        }

        private void SaveLocationsToFile()
        {
            try
            {
                var directory = Path.GetDirectoryName(_dataFilePath);
                if (!Directory.Exists(directory) && !string.IsNullOrEmpty(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                var json = JsonConvert.SerializeObject(_locations, Formatting.Indented);
                File.WriteAllText(_dataFilePath, json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving locations to file");
            }
        }

        public async Task<List<Location>> GetAllLocationsAsync()
        {
            // Simulate async operation
            return await Task.FromResult(_locations);
        }

        public async Task<Location> GetLocationByIdAsync(int id)
        {
            return await Task.FromResult(_locations.FirstOrDefault(l => l.Id == id));
        }

        public async Task<Location> AddLocationAsync(Location location)
        {
            // Assign new ID
            location.Id = _locations.Count > 0 ? _locations.Max(l => l.Id) + 1 : 1;

            _locations.Add(location);
            SaveLocationsToFile();

            return await Task.FromResult(location);
        }

        private List<Location> GetDefaultLocations()
        {
            return new List<Location>
            {
                new Location
                {
                    Id = 1,
                    Name = "City Pharmacy",
                    Type = "Pharmacy",
                    Address = "123 Main St",
                    AvailableTimeSlots = new List<TimeSlot>
                    {
                        new TimeSlot { DayOfWeek = DayOfWeek.Monday, OpenTime = new TimeSpan(9, 0, 0), CloseTime = new TimeSpan(18, 0, 0) },
                        new TimeSlot { DayOfWeek = DayOfWeek.Tuesday, OpenTime = new TimeSpan(9, 0, 0), CloseTime = new TimeSpan(18, 0, 0) },
                        new TimeSlot { DayOfWeek = DayOfWeek.Wednesday, OpenTime = new TimeSpan(9, 0, 0), CloseTime = new TimeSpan(18, 0, 0) },
                        new TimeSlot { DayOfWeek = DayOfWeek.Thursday, OpenTime = new TimeSpan(9, 0, 0), CloseTime = new TimeSpan(18, 0, 0) },
                        new TimeSlot { DayOfWeek = DayOfWeek.Friday, OpenTime = new TimeSpan(9, 0, 0), CloseTime = new TimeSpan(18, 0, 0) },
                        new TimeSlot { DayOfWeek = DayOfWeek.Saturday, OpenTime = new TimeSpan(10, 0, 0), CloseTime = new TimeSpan(14, 0, 0) }
                    }
                },
                new Location
                {
                    Id = 2,
                    Name = "Fresh Bakery",
                    Type = "Bakery",
                    Address = "456 Oak Ave",
                    AvailableTimeSlots = new List<TimeSlot>
                    {
                        new TimeSlot { DayOfWeek = DayOfWeek.Monday, OpenTime = new TimeSpan(6, 0, 0), CloseTime = new TimeSpan(18, 0, 0) },
                        new TimeSlot { DayOfWeek = DayOfWeek.Tuesday, OpenTime = new TimeSpan(6, 0, 0), CloseTime = new TimeSpan(18, 0, 0) },
                        new TimeSlot { DayOfWeek = DayOfWeek.Wednesday, OpenTime = new TimeSpan(6, 0, 0), CloseTime = new TimeSpan(18, 0, 0) },
                        new TimeSlot { DayOfWeek = DayOfWeek.Thursday, OpenTime = new TimeSpan(6, 0, 0), CloseTime = new TimeSpan(18, 0, 0) },
                        new TimeSlot { DayOfWeek = DayOfWeek.Friday, OpenTime = new TimeSpan(6, 0, 0), CloseTime = new TimeSpan(18, 0, 0) },
                        new TimeSlot { DayOfWeek = DayOfWeek.Saturday, OpenTime = new TimeSpan(7, 0, 0), CloseTime = new TimeSpan(14, 0, 0) },
                        new TimeSlot { DayOfWeek = DayOfWeek.Sunday, OpenTime = new TimeSpan(8, 0, 0), CloseTime = new TimeSpan(12, 0, 0) }
                    }
                },
                new Location
                {
                    Id = 3,
                    Name = "Style Cuts",
                    Type = "Barber Shop",
                    Address = "789 Elm St",
                    AvailableTimeSlots = new List<TimeSlot>
                    {
                        new TimeSlot { DayOfWeek = DayOfWeek.Monday, OpenTime = new TimeSpan(11, 0, 0), CloseTime = new TimeSpan(20, 0, 0) },
                        new TimeSlot { DayOfWeek = DayOfWeek.Tuesday, OpenTime = new TimeSpan(11, 0, 0), CloseTime = new TimeSpan(20, 0, 0) },
                        new TimeSlot { DayOfWeek = DayOfWeek.Wednesday, OpenTime = new TimeSpan(11, 0, 0), CloseTime = new TimeSpan(20, 0, 0) },
                        new TimeSlot { DayOfWeek = DayOfWeek.Thursday, OpenTime = new TimeSpan(11, 0, 0), CloseTime = new TimeSpan(20, 0, 0) },
                        new TimeSlot { DayOfWeek = DayOfWeek.Friday, OpenTime = new TimeSpan(10, 0, 0), CloseTime = new TimeSpan(22, 0, 0) },
                        new TimeSlot { DayOfWeek = DayOfWeek.Saturday, OpenTime = new TimeSpan(10, 0, 0), CloseTime = new TimeSpan(22, 0, 0) }
                    }
                },
                new Location
                {
                    Id = 4,
                    Name = "Mega Mart",
                    Type = "Supermarket",
                    Address = "101 Broadway",
                    AvailableTimeSlots = new List<TimeSlot>
                    {
                        new TimeSlot { DayOfWeek = DayOfWeek.Monday, OpenTime = new TimeSpan(8, 0, 0), CloseTime = new TimeSpan(22, 0, 0) },
                        new TimeSlot { DayOfWeek = DayOfWeek.Tuesday, OpenTime = new TimeSpan(8, 0, 0), CloseTime = new TimeSpan(22, 0, 0) },
                        new TimeSlot { DayOfWeek = DayOfWeek.Wednesday, OpenTime = new TimeSpan(8, 0, 0), CloseTime = new TimeSpan(22, 0, 0) },
                        new TimeSlot { DayOfWeek = DayOfWeek.Thursday, OpenTime = new TimeSpan(8, 0, 0), CloseTime = new TimeSpan(22, 0, 0) },
                        new TimeSlot { DayOfWeek = DayOfWeek.Friday, OpenTime = new TimeSpan(8, 0, 0), CloseTime = new TimeSpan(23, 0, 0) },
                        new TimeSlot { DayOfWeek = DayOfWeek.Saturday, OpenTime = new TimeSpan(8, 0, 0), CloseTime = new TimeSpan(23, 0, 0) },
                        new TimeSlot { DayOfWeek = DayOfWeek.Sunday, OpenTime = new TimeSpan(9, 0, 0), CloseTime = new TimeSpan(21, 0, 0) }
                    }
                },
                new Location
                {
                    Id = 5,
                    Name = "Sweet Tooth",
                    Type = "Candy Store",
                    Address = "202 Cherry Lane",
                    AvailableTimeSlots = new List<TimeSlot>
                    {
                        new TimeSlot { DayOfWeek = DayOfWeek.Monday, OpenTime = new TimeSpan(12, 0, 0), CloseTime = new TimeSpan(20, 0, 0) },
                        new TimeSlot { DayOfWeek = DayOfWeek.Tuesday, OpenTime = new TimeSpan(12, 0, 0), CloseTime = new TimeSpan(20, 0, 0) },
                        new TimeSlot { DayOfWeek = DayOfWeek.Wednesday, OpenTime = new TimeSpan(12, 0, 0), CloseTime = new TimeSpan(20, 0, 0) },
                        new TimeSlot { DayOfWeek = DayOfWeek.Thursday, OpenTime = new TimeSpan(12, 0, 0), CloseTime = new TimeSpan(20, 0, 0) },
                        new TimeSlot { DayOfWeek = DayOfWeek.Friday, OpenTime = new TimeSpan(12, 0, 0), CloseTime = new TimeSpan(22, 0, 0) },
                        new TimeSlot { DayOfWeek = DayOfWeek.Saturday, OpenTime = new TimeSpan(10, 0, 0), CloseTime = new TimeSpan(22, 0, 0) },
                        new TimeSlot { DayOfWeek = DayOfWeek.Sunday, OpenTime = new TimeSpan(12, 0, 0), CloseTime = new TimeSpan(18, 0, 0) }
                    }
                },
                new Location
                {
                    Id = 6,
                    Name = "Cinemax",
                    Type = "Cinema Complex",
                    Address = "303 Movie Blvd",
                    AvailableTimeSlots = new List<TimeSlot>
                    {
                        new TimeSlot { DayOfWeek = DayOfWeek.Monday, OpenTime = new TimeSpan(12, 30, 0), CloseTime = new TimeSpan(23, 0, 0) },
                        new TimeSlot { DayOfWeek = DayOfWeek.Tuesday, OpenTime = new TimeSpan(12, 30, 0), CloseTime = new TimeSpan(23, 0, 0) },
                        new TimeSlot { DayOfWeek = DayOfWeek.Wednesday, OpenTime = new TimeSpan(12, 30, 0), CloseTime = new TimeSpan(23, 0, 0) },
                        new TimeSlot { DayOfWeek = DayOfWeek.Thursday, OpenTime = new TimeSpan(12, 30, 0), CloseTime = new TimeSpan(23, 0, 0) },
                        new TimeSlot { DayOfWeek = DayOfWeek.Friday, OpenTime = new TimeSpan(12, 0, 0), CloseTime = new TimeSpan(1, 0, 0) },
                        new TimeSlot { DayOfWeek = DayOfWeek.Saturday, OpenTime = new TimeSpan(11, 0, 0), CloseTime = new TimeSpan(1, 0, 0) },
                        new TimeSlot { DayOfWeek = DayOfWeek.Sunday, OpenTime = new TimeSpan(11, 0, 0), CloseTime = new TimeSpan(23, 0, 0) }
                    }
                },
                new Location
                {
                    Id = 7,
                    Name = "Downtown Library",
                    Type = "Library",
                    Address = "404 Book St",
                    AvailableTimeSlots = new List<TimeSlot>
                    {
                        new TimeSlot { DayOfWeek = DayOfWeek.Monday, OpenTime = new TimeSpan(9, 0, 0), CloseTime = new TimeSpan(17, 0, 0) },
                        new TimeSlot { DayOfWeek = DayOfWeek.Tuesday, OpenTime = new TimeSpan(9, 0, 0), CloseTime = new TimeSpan(17, 0, 0) },
                        new TimeSlot { DayOfWeek = DayOfWeek.Wednesday, OpenTime = new TimeSpan(9, 0, 0), CloseTime = new TimeSpan(17, 0, 0) },
                        new TimeSlot { DayOfWeek = DayOfWeek.Thursday, OpenTime = new TimeSpan(9, 0, 0), CloseTime = new TimeSpan(17, 0, 0) },
                        new TimeSlot { DayOfWeek = DayOfWeek.Friday, OpenTime = new TimeSpan(9, 0, 0), CloseTime = new TimeSpan(17, 0, 0) },
                        new TimeSlot { DayOfWeek = DayOfWeek.Saturday, OpenTime = new TimeSpan(10, 0, 0), CloseTime = new TimeSpan(14, 0, 0) }
                    }
                },
                new Location
                {
                    Id = 8,
                    Name = "Central Park Cafe",
                    Type = "Cafe",
                    Address = "505 Park Ave",
                    AvailableTimeSlots = new List<TimeSlot>
                    {
                        new TimeSlot { DayOfWeek = DayOfWeek.Monday, OpenTime = new TimeSpan(7, 0, 0), CloseTime = new TimeSpan(17, 0, 0) },
                        new TimeSlot { DayOfWeek = DayOfWeek.Tuesday, OpenTime = new TimeSpan(7, 0, 0), CloseTime = new TimeSpan(17, 0, 0) },
                        new TimeSlot { DayOfWeek = DayOfWeek.Wednesday, OpenTime = new TimeSpan(7, 0, 0), CloseTime = new TimeSpan(17, 0, 0) },
                        new TimeSlot { DayOfWeek = DayOfWeek.Thursday, OpenTime = new TimeSpan(7, 0, 0), CloseTime = new TimeSpan(17, 0, 0) },
                        new TimeSlot { DayOfWeek = DayOfWeek.Friday, OpenTime = new TimeSpan(7, 0, 0), CloseTime = new TimeSpan(17, 0, 0) },
                        new TimeSlot { DayOfWeek = DayOfWeek.Saturday, OpenTime = new TimeSpan(8, 0, 0), CloseTime = new TimeSpan(17, 0, 0) },
                        new TimeSlot { DayOfWeek = DayOfWeek.Sunday, OpenTime = new TimeSpan(9, 0, 0), CloseTime = new TimeSpan(15, 0, 0) }
                    }
                },
                new Location
                {
                    Id = 9,
                    Name = "Fitness First",
                    Type = "Gym",
                    Address = "606 Muscle Way",
                    AvailableTimeSlots = new List<TimeSlot>
                    {
                        new TimeSlot { DayOfWeek = DayOfWeek.Monday, OpenTime = new TimeSpan(6, 0, 0), CloseTime = new TimeSpan(23, 0, 0) },
                        new TimeSlot { DayOfWeek = DayOfWeek.Tuesday, OpenTime = new TimeSpan(6, 0, 0), CloseTime = new TimeSpan(23, 0, 0) },
                        new TimeSlot { DayOfWeek = DayOfWeek.Wednesday, OpenTime = new TimeSpan(6, 0, 0), CloseTime = new TimeSpan(23, 0, 0) },
                        new TimeSlot { DayOfWeek = DayOfWeek.Thursday, OpenTime = new TimeSpan(6, 0, 0), CloseTime = new TimeSpan(23, 0, 0) },
                        new TimeSlot { DayOfWeek = DayOfWeek.Friday, OpenTime = new TimeSpan(6, 0, 0), CloseTime = new TimeSpan(23, 0, 0) },
                        new TimeSlot { DayOfWeek = DayOfWeek.Saturday, OpenTime = new TimeSpan(8, 0, 0), CloseTime = new TimeSpan(20, 0, 0) },
                        new TimeSlot { DayOfWeek = DayOfWeek.Sunday, OpenTime = new TimeSpan(8, 0, 0), CloseTime = new TimeSpan(20, 0, 0) }
                    }
                },
                new Location
                {
                    Id = 10,
                    Name = "Golden Spa",
                    Type = "Spa",
                    Address = "707 Relaxation Rd",
                    AvailableTimeSlots = new List<TimeSlot>
                    {
                        new TimeSlot { DayOfWeek = DayOfWeek.Monday, OpenTime = new TimeSpan(10, 0, 0), CloseTime = new TimeSpan(19, 0, 0) },
                        new TimeSlot { DayOfWeek = DayOfWeek.Tuesday, OpenTime = new TimeSpan(10, 0, 0), CloseTime = new TimeSpan(19, 0, 0) },
                        new TimeSlot { DayOfWeek = DayOfWeek.Wednesday, OpenTime = new TimeSpan(10, 0, 0), CloseTime = new TimeSpan(19, 0, 0) },
                        new TimeSlot { DayOfWeek = DayOfWeek.Thursday, OpenTime = new TimeSpan(10, 0, 0), CloseTime = new TimeSpan(19, 0, 0) },
                        new TimeSlot { DayOfWeek = DayOfWeek.Friday, OpenTime = new TimeSpan(10, 0, 0), CloseTime = new TimeSpan(21, 0, 0) },
                        new TimeSlot { DayOfWeek = DayOfWeek.Saturday, OpenTime = new TimeSpan(9, 0, 0), CloseTime = new TimeSpan(21, 0, 0) },
                        new TimeSlot { DayOfWeek = DayOfWeek.Sunday, OpenTime = new TimeSpan(12, 0, 0), CloseTime = new TimeSpan(18, 0, 0) }
                    }
                }
            };
        }
    }
}