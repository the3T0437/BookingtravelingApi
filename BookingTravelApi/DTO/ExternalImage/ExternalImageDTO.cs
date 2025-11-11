using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using BookingTravelApi.DTO.DayActivity;
using BookingTravelApi.Infrastructure;
using Org.BouncyCastle.Security;

namespace BookingTravelApi.DTO.ExternalImage
{
    public class ExternalImageDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Image { get; set; } = null!;

        [Required]
        public string DeletePath { get; set; } = null!;
    }

    public static class ExternalImageExtension
    {
        public static ExternalImageDTO Map(this Domains.ExternalImage image)
        {
            var Host = Environment.GetEnvironmentVariable("Host");
            var imagePath = $"http://{Host}{AppConfig.GetRequestImagePath()}/{image.Path}";
            var deleteImagePath = $"http://{Host}{AppConfig.DeleteImagePath()}/{image.Id}";

            return new ExternalImageDTO()
            {
                Id = image.Id,
                Image = imagePath,
                DeletePath = deleteImagePath,
            };
        }
    }
}