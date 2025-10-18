using BookingTravelApi.Domains;
using BookingTravelApi.DTO.tourlocation;

namespace BookingTravelApi.Extensions
{
    public static class TourLocationExtention
    {
        public static TourLocationDTO Map(this TourLocation tourLocation)
        {
            return new TourLocationDTO()
            {
                TourId = tourLocation.TourId,
                LocationId = tourLocation.LocationId,
                Location = tourLocation.Location!.Map()
            };
        }
    }
}