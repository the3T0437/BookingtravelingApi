using BookingTravelApi.Domains;
using BookingTravelApi.DTO.DayOfTour;

namespace BookingTravelApi.Extensions
{
    public static class DayOfTourExtension
    {
        public static DayOfTourDTO Map(this DayOfTour dayOfTour)
        {
            return new DayOfTourDTO()
            {
                Title = dayOfTour.Title,
                Description = dayOfTour.Description,
                DayActivities = dayOfTour.DayActivities?.Select(i => i.Map()).ToList() ?? []
            };
        }
    }
}