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
                ScheduleId = guide.ScheduleId,

                User = guide.Staff!.User!.Map(),
                Tour = guide.Schedule!.Tour!.Map(),
                Schedule = guide.Schedule!.Map(),
                TourLocation = guide.Schedule!.Tour!.TourLocations!.Select(i => i.Map()).ToList()
            };
        }
    }
}
