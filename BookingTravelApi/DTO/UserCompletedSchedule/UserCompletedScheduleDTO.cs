using BookingTravelApi.Domains;
using BookingTravelApi.DTO.booking;
using BookingTravelApi.DTO.schedule;
using BookingTravelApi.DTO.Tour;
using BookingTravelApi.DTO.user;

namespace BookingTravelApi.DTO.usercompletedschedule
{
    public class UserCompletedScheduleDTO
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public String Code { get; set; } = null!;
        public String? Name { get; set; } = null!;
        public String AvatarPath { get; set; } = null!;
        public List<BookingDTO> Booking { get; set; } = null!;

    }
}
