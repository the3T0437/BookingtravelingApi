using System.ComponentModel.DataAnnotations;
using BookingTravelApi.DTO.place;

namespace BookingTravelApi.DTO.ScheduleAssignmentDTO
{
    public class ScheduleAssignmentDTO
    {
        [Required]
        public int idSchedule { get; set; }

        [Required]
        public String titleTour { get; set; } = null!;


        public List<String> tourImages { get; set; } = null!;

        public List<string> nameLocations { get; set; } = null!;

        public List<String> placeNames { get; set; } = null!;
    }
}
