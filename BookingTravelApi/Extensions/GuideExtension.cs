using BookingTravelApi.Domains;
using BookingTravelApi.DTO.guide;

namespace BookingTravelApi.Extensions
{
    public static class GuideExtension
    {
        public static GuideDTO Map(this Guide guide)
        {
            return new GuideDTO()
            {
                StaffId = guide.StaffId,
                ScheduleId = guide.ScheduleId
            };
        }
    }
}
