namespace BookingTravelApi.DTO.notification
{
    public class NotificationDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public String Content { get; set; } = null!;
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}