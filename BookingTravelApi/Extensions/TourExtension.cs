using BookingTravelApi.Domains;
using BookingTravelApi.DTO.Tour;

namespace BookingTravelApi.Extensions
{
    public static class TourExtension
    {
        public static TourDTO Map(this Tour tour)
        {
            return new TourDTO()
            {
                Day = tour.Day,
                Title = tour.Title,
                Price = tour.Price,
                Description = tour.Description,

                DayOfTours = tour.DayOfTours?.Select(i => i.Map()).ToList() ?? [],
                TourImages = tour.TourImages?.Select(i => i.Path).ToList() ?? []
            };
        }
    }
}