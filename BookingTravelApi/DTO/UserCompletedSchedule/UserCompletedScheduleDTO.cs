using BookingTravelApi.Domains;
using BookingTravelApi.DTO.booking;
using BookingTravelApi.DTO.schedule;
using BookingTravelApi.DTO.Tour;
using BookingTravelApi.DTO.user;

namespace BookingTravelApi.DTO.usercompletedschedule
{
    public class UserCompletedScheduleDTO
    {
        public int BookingId {get; set;}
        public ScheduleDTO? Schedule {get; set;}
        public BookingDTO? Booking {get; set;}

    }
}
