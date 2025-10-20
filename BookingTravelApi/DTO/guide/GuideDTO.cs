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

        public UserDTO UserDTO { get; set; } = null!;
        public ScheduleDTO ScheduleDTO { get; set; } = null!;
        public List<TourLocationDTO> TourLocationDTO { get; set; } = null!;
        public TourDTO TourDTO { get; set; } = null!;
        public StaffDTO StaffDTO { get; set; } = null!;
    }
}
