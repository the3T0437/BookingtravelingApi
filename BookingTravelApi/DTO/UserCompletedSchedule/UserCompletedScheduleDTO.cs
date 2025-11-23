using BookingTravelApi.Domains;
using BookingTravelApi.DTO.actualCashDTO;
using BookingTravelApi.DTO.booking;
using BookingTravelApi.DTO.schedule;
using BookingTravelApi.DTO.Tour;
using BookingTravelApi.DTO.user;

namespace BookingTravelApi.DTO.usercompletedschedule
{
    public class UserCompletedScheduleDTO
    {
        public int CountPeople {get; set;}
        public ActualCashDTO? Actualcashs {get; set;}
        public BookingDTO? Booking {get; set;}

    }
}
