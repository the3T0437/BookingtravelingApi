using BookingTravelApi.Domains;
using BookingTravelApi.DTO.DayActivity;

namespace BookingTravelApi.Extensions
{
    public static class DayActivityExtension
    {
        public static DayActivityDTO Map(this DayActivity dayActivity)
        {
            return new DayActivityDTO()
            {
                Activity = dayActivity.Activity?.Map(),
                LocationActivity = dayActivity.LocationActivity?.Map(),
                Time = dayActivity.Time
            };
        }
    }
}