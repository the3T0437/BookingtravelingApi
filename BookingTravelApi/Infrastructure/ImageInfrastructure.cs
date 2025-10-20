using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;

namespace BookingTravelApi.Infrastructure
{
    public class ImageInfrastructure
    {
        public static async Task<List<String>> WriteImages(List<String> imagesBase64)
        {
            List<String> paths = [];
            var pathsTask = imagesBase64.Select(async i =>
            {
                var path = await ImageInfrastructure.WriteImage(i);
                if (path == null)
                {
                    paths.ForEach(i => ImageInfrastructure.DeleteImage(i));
                    throw new Exception("Error while write image");
                }
                paths.Add(path!);
                return path;
            }).ToList();

            var returnPaths = (await Task.WhenAll(pathsTask)).ToList();
            return returnPaths;
        }

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

        public static void DeleteImages(List<String> paths)
        {
            foreach(var path in paths)
            {
                DeleteImage(path);
            }
        }

        public static void DeleteImage(String path)
        {
            File.Delete(path);
        }
    }
}