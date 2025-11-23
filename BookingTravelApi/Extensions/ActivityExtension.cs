using BookingTravelApi.Domains;
using BookingTravelApi.DTO.Activity;

namespace BookingTravelApi.Extensions
{
    public static class ActivityExtension
    {
        public static ActivityDTO Map(this Activity activity)
        {
            return new ActivityDTO()
            {
                Id = activity.Id,
                Action = activity.Action
            };
        }
    }
}
