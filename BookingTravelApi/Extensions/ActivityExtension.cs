using BookingTravelApi.Domains;
using BookingTravelApi.DTO.Activity;
using BookingTravelApi.DTO.LocationActivity;

namespace BookingTravelApi.Extensions
{
    public static class ActivityExtension
    {
        public static ActivityDTO Map(this Activity activity)
        {
            return new ActivityDTO()
            {
                Action = activity.Action
            };
        }
    }
}
