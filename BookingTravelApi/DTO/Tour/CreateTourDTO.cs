using System.ComponentModel.DataAnnotations;
using BookingTravelApi.Domains;
using BookingTravelApi.DTO.DayOfTour;
using BookingTravelApi.Infrastructure;
using Microsoft.Net.Http.Headers;

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
        public int PercentDeposit { get; set; }
        [Required]
        public String Description { get; set; } = null!;
        [Required]
        public List<CreateDayOfTourDTO> DayOfTours { get; set; } = null!;
        public List<String> TourImages { get; set; } = [];

    }

    public static class CreateTourDTOExtension
    {
        public static async Task<Domains.Tour> Map(this CreateTourDTO createTourDTO)
        {
            var dayOfTours = createTourDTO.DayOfTours.Select(i => i.Map()).ToList();
            List<String> paths = await ImageInfrastructure.WriteImages(createTourDTO.TourImages);
            var tourImages = paths.Select(i => new TourImage() { Path = i }).ToList();

            return new Domains.Tour()
            {
                Day = createTourDTO.Day,
                Title = createTourDTO.Title,
                Price = createTourDTO.Price,
                PercentDeposit = createTourDTO.PercentDeposit,
                Description = createTourDTO.Description,
                DayOfTours = dayOfTours,
                TourImages = tourImages,
                TotalReviews = 0,
                TotalStars = 0
            };
        }
    }
}