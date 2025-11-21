using System.ComponentModel.DataAnnotations;
using BookingTravelApi.Domains;
using BookingTravelApi.Helpers;

namespace BookingTravelApi.DTO.notification
{
    public class CreateNotificationDTO
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public String Content { get; set; } = null!;

        public Notification Map()
        {
            return new Notification()
            {
                UserId = UserId,
                Content = Content,
                IsRead = false,
                CreatedAt = DateTimeHelper.GetVietNamTime(),
            };
        }
    }
}