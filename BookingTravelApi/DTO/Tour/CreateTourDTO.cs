using System.ComponentModel.DataAnnotations;
using BookingTravelApi.DTO.DayOfTour;

namespace BookingTravelApi.DTO.Tour
{
    public class CreateTourDTO
    {
        [Required]
        public int Day { get; set; }
        [Required]
        public String Title { get; set; } = null!;
        [Required]
        public int Price { get; set; }
        [Required]
        public String Description { get; set; } = null!;
        [Required]
        public List<CreateDayOfTourDTO> DayOfTours { get; set; } = null!;
        [Required]
        public List<IFormFile> TourImages { get; set; } = null!;

    }

    public static class CreateTourDTOExtension
    {
        public static Domains.Tour Map(this CreateTourDTO createTourDTO)
        {
            return new Domains.Tour()
            {
                Day = createTourDTO.Day,
                Title = createTourDTO.Title,
                Price = createTourDTO.Price,
                Description = createTourDTO.Description
            };
        }
    }
}