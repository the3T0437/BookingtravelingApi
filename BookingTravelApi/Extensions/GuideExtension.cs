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
                ScheduleDTO = guide.Schedule!.Map(),
                TourDTO = guide.Schedule!.Tour!.Map(),
                UserCompletedScheduleDTO = guide.Schedule.UserCompletedSchedules!.Select(i => i.Map()).ToList()
            };
        }
    }
}
