using BookingTravelApi.DTO.Tour;

namespace BookingTravelApi.DTO.schedule
{
    public class ScheduleDTOOfAdmin : ScheduleDTO
    {
        public int TotalReviews { get; set; }
        public int TotalStars { get; set; }
    }
}