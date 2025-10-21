using System.Runtime.ConstrainedExecution;

namespace BookingTravelApi.Infrastructure
{
    public class AppConfig
    {
        public static String GetImagePath()
        {
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "images");
            return imagePath;
        }

        public static String GetRequestImagePath()
        {
            return "/images";
        }
    }
}
