using BookingTravelApi.Domains;
using BookingTravelApi.DTO.schedule;
using BookingTravelApi.DTO.Tour;
using BookingTravelApi.DTO.tourlocation;
using BookingTravelApi.DTO.user;

namespace BookingTravelApi.DTO.usercompletedschedule
{
    public class UserCompletedScheduleDTO
    {
        public int UserId { get; set; }
        public int ScheduleId { get; set; }
        
        public TourDTO Tour { get; set; } = null!;
        public ScheduleDTO Schedule { get; set; } = null!;
        public List<TourLocationDTO> TourLocation { get; set; } = null!;
    }
}
