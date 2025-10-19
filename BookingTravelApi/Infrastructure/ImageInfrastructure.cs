using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;

namespace BookingTravelApi.Infrastructure
{
    public class ImageInfrastructure
    {
        public static async Task<String?> WriteImage(String imageBase64)
        {
            var image = Convert.FromBase64String(imageBase64);
            if (image.IsNullOrEmpty())
            {
                return null;
            }

            // Define the path where you want to save the image
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Generate a unique file name to avoid conflicts
            var uniqueFileName = Guid.NewGuid().ToString() + ".jpg";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Save the image to the specified path
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await stream.WriteAsync(image);
            }

            return filePath;
        }

        public static void DeleteImage(String path)
        {
            Directory.Delete(path);
        }
    }
}