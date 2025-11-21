using System.ComponentModel.DataAnnotations;
using BookingTravelApi.Domains;

namespace BookingTravelApi.DTO.notification
{
    public class CreateNotificationDTO
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public String Content { get; set; } = null!;

        [Required]
        public DateTime CreatedAt { get; set; }

        public Notification Map()
        {
            return new Notification()
            {
                UserId = UserId,
                Content = Content,
                IsRead = false,
                CreatedAt = CreatedAt == default ? DateTime.UtcNow.AddHours(7) : CreatedAt,
            };
        }
    }
}