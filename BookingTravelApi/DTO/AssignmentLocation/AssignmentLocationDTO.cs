using System.ComponentModel.DataAnnotations;
using BookingTravelApi.DTO.place;

namespace BookingTravelApi.DTO.AssignmentDTO
{
    public class AssignmentDTO
    {
        [Required]
        public int IdSchedule { get; set; }

        [Required]
        public String TitleTour { get; set; } = null!;


        public List<String> TourImageDTOs { get; set; } = null!;

        public List<string> NameLocations { get; set; } = null!;

        public List<String> placeNameDTOs { get; set; } = null!;
    }
}
