using BookingTravelApi.Domains;
using BookingTravelApi.DTO.usercompletedschedule;

namespace BookingTravelApi.Extensions
{
    public static class UserCompletedScheduleExtension
    {
        public static UserCompletedScheduleDTO Map(this UserCompletedSchedule userCompletedSchedule)
        {
            return new UserCompletedScheduleDTO()
            {
                CountPeople = userCompletedSchedule.countPeople, 
                Booking = userCompletedSchedule.Booking!.Map()
            };
        }
    }
}