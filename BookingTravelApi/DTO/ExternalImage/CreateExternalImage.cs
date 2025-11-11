using System.ComponentModel.DataAnnotations;
using BookingTravelApi.DTO.DayActivity;
using BookingTravelApi.Infrastructure;
using Org.BouncyCastle.Security;

namespace BookingTravelApi.DTO.ExternalImage
{
    public class CreateExternalImageDTO
    {
        [Required]
        public string Image { get; set; } = null!;
    }

    public static class CreateExternalImageDTOExtension
    {
        public static async Task<Domains.ExternalImage> Map(this CreateExternalImageDTO externalImageDTO)
        {
            var path = await ImageInfrastructure.WriteImage(externalImageDTO.Image);
            if (path == null)
            {
                throw new InvalidParameterException("image wrong format");
            }
            
            return new Domains.ExternalImage()
            {
                Path = path,
            };
        }
    }
}