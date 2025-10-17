using BookingTravelApi.DTO.DayActivity;

namespace BookingTravelApi.DTO.DayOfTour
{
    public class DayOfTourDTO
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;

        public List<DayActivityDTO> DayActivities { get; set; } = [];
    }
}