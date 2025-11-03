using BookingTravelApi.Domains;
using BookingTravelApi.DTO.notification;

namespace BookingTravelApi.Extensions
{
    public static class NotificationExtension
    {
        public static NotificationDTO Map(this Notification notification)
        {
            return new NotificationDTO()
            {
                Id = notification.Id,
                UserId = notification.UserId,
                Content = notification.Content,
                IsRead = notification.IsRead,
                CreatedAt = notification.CreatedAt
            };
        }
    }
}