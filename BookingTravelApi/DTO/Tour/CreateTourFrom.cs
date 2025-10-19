/*
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using BookingTravelApi.Domains;
using BookingTravelApi.DTO.DayOfTour;
using BookingTravelApi.Infrastructure;

namespace BookingTravelApi.DTO.Tour
{
    public class CreateTourForm
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
        public String DayOfTours { get; set; } = null!;
        public List<IFormFile> TourImages { get; set; } = [];

    }

    public static class CreateTourFormExtension
    {
        public static CreateTourDTO Map(this CreateTourForm createTourFrom)
        {
            var dayOfTours = JsonSerializer.Deserialize<List<CreateDayOfTourDTO>>(createTourFrom.DayOfTours);
            if (dayOfTours == null)
                throw new Exception("cant parse list dayOfTours");
            return new CreateTourDTO()
            {
                Day = createTourFrom.Day,
                Title = createTourFrom.Title,
                Price = createTourFrom.Price,
                Description = createTourFrom.Description,
                DayOfTours = dayOfTours,
                TourImages = createTourFrom.TourImages,
            };
        }
    }
}
*/