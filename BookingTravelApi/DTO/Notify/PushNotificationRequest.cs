using System.ComponentModel.DataAnnotations;

namespace BookingTravelApi.DTO
{
    public class PushNotificationRequest
{
    [Required]
    public int userId { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
}
}