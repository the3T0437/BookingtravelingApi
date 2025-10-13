using System.ComponentModel.DataAnnotations;

namespace BookingTravelApi.Domains
{
    public class Review
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [Required]
        [MaxLength(10000)]
        public String Content { get; set; } = null!;

        [Required]
        public int ScheduleId { get; set; } 

        [Required]
        public DateTime CreatedAt { get; set; }
    }
}
