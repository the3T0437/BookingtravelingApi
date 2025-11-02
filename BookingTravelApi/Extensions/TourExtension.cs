using BookingTravelApi.Domains;
using BookingTravelApi.DTO.Tour;
using BookingTravelApi.Infrastructure;

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

            var setPlaces = places.ToHashSet().ToList();
            var reviewCount = tour.Schedules?.Where(i => i.Reviews != null).SelectMany(i => i.Reviews ?? []).Count();
            var starCount = tour.Schedules?.Where(i => i.Reviews != null).SelectMany(i => i.Reviews ?? []).Select(i => i.Rating).Sum();

            var Host = Environment.GetEnvironmentVariable("Host");

            return new TourDTO()
            {
                Id = tour.Id,
                Day = tour.Day,
                Title = tour.Title,
                Price = tour.Price,
                PercentDeposit = tour.PercentDeposit,
                Description = tour.Description,

                DayOfTours = tour.DayOfTours?.Select(i => i.Map()).ToList() ?? [],
                TourImages = tour.TourImages?.Select(i => $"http://{Host}{AppConfig.GetRequestImagePath()}/{i.Path}").ToList() ?? [],
                Locations = locations.ToList().Select(i => i.Map()).ToList(),
                Places = setPlaces.Select(i => i!.Map()).ToList(),

                TotalReviews = tour.TotalReviews,
                TotalStars = tour.TotalStars,
            };
        }
    }
}