using System.ComponentModel.DataAnnotations;

namespace BookingTravelApi.Domains
{
    public class Notification
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public String Content { get; set; } = null!;

        [Required]
        public bool IsRead { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public User? User { get; set; }

    }
}
