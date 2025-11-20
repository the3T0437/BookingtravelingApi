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
                BookingId = userCompletedSchedule.BookingId,
                Schedule = userCompletedSchedule.Booking!.Schedule!.Map(),
                Booking = userCompletedSchedule.Booking.Map()
            };
        }
    }
}