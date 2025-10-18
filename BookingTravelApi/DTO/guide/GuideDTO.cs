using BookingTravelApi.DTO.schedule;
using BookingTravelApi.DTO.staff;
using BookingTravelApi.DTO.Tour;
using BookingTravelApi.DTO.tourlocation;
using BookingTravelApi.DTO.user;

namespace BookingTravelApi.DTO.guide
{
    public class GuideDTO
    {
        public int StaffId { get; set; }
        public int ScheduleId { get; set; }

        public UserDTO User { get; set; } = null!;
        public ScheduleDTO Schedule { get; set; } = null!;
        public List<TourLocationDTO> TourLocation { get; set; } = null!;
        public TourDTO Tour { get; set; } = null!;
        public StaffDTO Staff { get; set; } = null!;
    }
}
