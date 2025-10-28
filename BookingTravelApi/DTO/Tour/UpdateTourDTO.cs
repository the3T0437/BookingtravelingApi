using System.ComponentModel.DataAnnotations;
using BookingTravelApi.DTO.DayOfTour;

namespace BookingTravelApi.DTO.Tour
{
    public class UpdateTourDTO
    {
        [Required]
        public int Id { get; set; }
        public String? Title { get; set; }
        public int? Price { get; set; }
        public int? PercentDeposit { get; set; }
        public String? Description { get; set; }

        public List<CreateDayOfTourDTO>? DayOfTours { get; set; }
        public List<String>? TourImages { get; set; }
        public List<String>? RetainImages { get; set; }
    }
}