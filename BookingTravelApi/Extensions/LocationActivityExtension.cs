using BookingTravelApi.Domains;
using BookingTravelApi.DTO.LocationActivity;

namespace BookingTravelApi.Extensions
{
    public static class LocationActivityExtension
    {
        public static LocationActivityDTO Map(this LocationActivity locationActivity)
        {
            return new LocationActivityDTO()
            {
                Id = locationActivity.Id,
                Name = locationActivity.Name
            };
        }
    }
}
