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

                UserDTO = guide.Staff!.User!.Map(),
                TourDTO = guide.Schedule!.Tour!.Map(),
                ScheduleDTO = guide.Schedule!.Map(),
                TourLocationDTO = guide.Schedule!.Tour!.TourLocations!.Select(i => i.Map()).ToList()
            };
        }
    }
}
