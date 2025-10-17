using BookingTravelApi.DTO.Location;

namespace BookingTravelApi.DTO.place
{
    public class PlaceDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public LocationDTO? Location { get; set; } = null!;
    }
}
