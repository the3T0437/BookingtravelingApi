using BookingTravelApi.DTO.Activity;
using BookingTravelApi.DTO.place;

namespace BookingTravelApi.DTO.LocationActivity
{
    public class LocationActivityDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public PlaceDTO? Place { get; set; } = null!;
    }
}
