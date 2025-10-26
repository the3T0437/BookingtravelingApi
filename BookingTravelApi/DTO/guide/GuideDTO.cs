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

        public ScheduleDTO ScheduleDTO { get; set; } = null!;
        public TourDTO TourDTO { get; set; } = null!;
        public List<UserCompletedScheduleDTO> UserCompletedScheduleDTO { get; set; } = null!;

    }
}
