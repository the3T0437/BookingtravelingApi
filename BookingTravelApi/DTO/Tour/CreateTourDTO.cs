using System.ComponentModel.DataAnnotations;
using BookingTravelApi.Domains;
using BookingTravelApi.DTO.DayOfTour;
using BookingTravelApi.Infrastructure;

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
        public List<String> TourImages { get; set; } = [];

    }

    public static class CreateTourDTOExtension
    {
        public static async Task<Domains.Tour> Map(this CreateTourDTO createTourDTO)
        {
            var dayOfTours = createTourDTO.DayOfTours.Select(i => i.Map()).ToList();
            List<String> paths = [];
            var tourImagesTask = createTourDTO.TourImages.Select(async i =>
            {
                var path = await ImageInfrastructure.WriteImage(i);
                if (path == null)
                {
                    paths.ForEach(i => ImageInfrastructure.DeleteImage(i));
                    throw new Exception("Error while write image");
                }
                paths.Add(path!);
                return new TourImage() { Path = path };
            }).ToList();

            var tourImages = (await Task.WhenAll(tourImagesTask)).ToList();

            return new Domains.Tour()
            {
                Day = createTourDTO.Day,
                Title = createTourDTO.Title,
                Price = createTourDTO.Price,
                Description = createTourDTO.Description,
                DayOfTours = dayOfTours,
                TourImages = tourImages
            };
        }
    }
}