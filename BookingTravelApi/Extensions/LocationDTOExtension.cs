using BookingTravelApi.Domains;
using BookingTravelApi.DTO.Location;

namespace BookingTravelApi.Extensions
{
    public static class LocationDTOExtension
    {
        public static LocationDTO Map(this Location location)
        {
            return new LocationDTO()
            {
                Id = location.Id,
                Name = location.Name
            };
        }
    }
}
