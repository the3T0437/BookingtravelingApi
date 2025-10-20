using BookingTravelApi.Domains;
using BookingTravelApi.DTO.Tour;

namespace BookingTravelApi.Extensions
{
    public static class TourExtension
    {
        public static TourDTO Map(this Tour tour)
        {
            var dayActivities = tour.DayOfTours?.SelectMany(i => i.DayActivities).ToList() ?? [];
            var locationActivities = dayActivities?.Select(i => i.LocationActivity).ToList();
            var places = locationActivities.Select(i => i.Place).ToList();
            var locations = places.Select(i => i.Location).ToHashSet();

            return new TourDTO()
            {
                Id = tour.Id,
                Day = tour.Day,
                Title = tour.Title,
                Price = tour.Price,
                Description = tour.Description,

                DayOfTours = tour.DayOfTours?.Select(i => i.Map()).ToList() ?? [],
                TourImages = tour.TourImages?.Select(i => i.Path).ToList() ?? [],
                Locations = locations.ToList().Select(i => i.Map()).ToList()
            };
        }
    }
}