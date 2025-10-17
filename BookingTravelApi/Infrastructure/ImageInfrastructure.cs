namespace BookingTravelApi.Infrastructure
{
    public class ImageInfrastructure
    {
        public static async Task<String?> WriteImage(IFormFile image)
        {
            if (image == null || image.Length == 0)
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
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Save the image to the specified path
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            return filePath;
        }
    }
}