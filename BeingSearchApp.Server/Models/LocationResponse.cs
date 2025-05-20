namespace BeingSearchApp.Server.Models
{
    public class LocationResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Address { get; set; }
        public TimeSpan OpenTime { get; set; }
        public TimeSpan CloseTime { get; set; }
        public bool IsAvailableBetween10And1 { get; set; }
    }
    
    public class LocationsResponseWrapper
    {
        public List<LocationResponse> Locations { get; set; }
        public int TotalCount { get; set; }
        public string Message { get; set; }
    }
}
