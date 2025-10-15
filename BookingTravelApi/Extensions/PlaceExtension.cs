using BookingTravelApi.Domains;
using BookingTravelApi.DTO.Location;
using BookingTravelApi.DTO.place;

namespace BookingTravelApi.Extensions
{
    public static class PlaceExtension
    {
        public static PlaceDTO Map(this Place place)
        {
            return new PlaceDTO()
            {
                Id = place.Id,
                Name = place.Name
            };
        }
    }
}
