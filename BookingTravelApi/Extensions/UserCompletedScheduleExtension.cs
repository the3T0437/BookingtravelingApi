using BookingTravelApi.Domains;
using BookingTravelApi.DTO.usercompletedschedule;
using BookingTravelApi.DTO.tourlocation;

namespace BookingTravelApi.Extensions
{
    public static class UserCompletedScheduleExtension
    {
        public static UserCompletedScheduleDTO Map(this UserCompletedSchedule userCompletedSchedule)
        {
            return new UserCompletedScheduleDTO()
            {
                UserId = userCompletedSchedule.UserId,
                ScheduleId = userCompletedSchedule.ScheduleId,

                Tour = userCompletedSchedule.Schedule!.Tour!.Map(),
                Schedule = userCompletedSchedule.Schedule!.Map(),
                TourLocation = userCompletedSchedule.Schedule!.Tour!.TourLocations!.Select(i => i.Map()).ToList()
            };
        }
    }
}
