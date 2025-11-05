using System.ComponentModel.DataAnnotations;
using BookingTravelApi.Domains;

namespace BookingTravelApi.DTO.notification
{
    public class UpdateNotificationDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public bool IsRead { get; set; }

        public void UpdateEntity(Notification notification)
        {
            notification.IsRead = IsRead;
        }
    }
}