using System.ComponentModel.DataAnnotations;
using BookingTravelApi.DTO.DayOfTour;

namespace BookingTravelApi.DTO.Tour
{
    public class TourDTO
    {
        public int Id { get; set; }
        public int Day { get; set; }
        public String Title { get; set; } = null!;
        public int Price { get; set; }
        public String Description { get; set; } = null!;

        public List<DayOfTourDTO> DayOfTours { get; set; } = null!;
        public List<String> TourImages { get; set; } = null!;
    }
}