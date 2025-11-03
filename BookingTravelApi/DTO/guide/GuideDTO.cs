using BookingTravelApi.DTO.booking;
using BookingTravelApi.DTO.schedule;
using BookingTravelApi.DTO.staff;
using BookingTravelApi.DTO.Tour;
using BookingTravelApi.DTO.tourlocation;
using BookingTravelApi.DTO.user;
using BookingTravelApi.DTO.usercompletedschedule;

namespace BookingTravelApi.DTO.guide
{
    public class GuideDTO
    {
        public String NameStaff { get; set; } = null!;
        public ScheduleDTO Schedule { get; set; } = null!;
        public List<BookingDTO> Booking { get; set; } = null!;      
             
    }
}
