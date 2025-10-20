
using BookingTravelApi.DTO.Location;

namespace BookingTravelApi.DTO.tourlocation
{
    public class TourLocationDTO
    {
        public int TourId { get; set; }
        public int LocationId { get; set; }
        public LocationDTO Location { get; set; } = null!;
    }
}