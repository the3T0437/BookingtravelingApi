using System.ComponentModel.DataAnnotations;
using BookingTravelApi.Domains;

namespace BookingTravelApi.DTO.review
{
    public class CreateReviewDTO
    {
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

        public Review Map()
        {
            return new Review()
            {
                UserId = UserId,
                Rating = Rating,
                Content = Content,
                ScheduleId = ScheduleId,
                CreatedAt = CreatedAt == default ? DateTime.UtcNow.AddHours(7) : CreatedAt,
            };
        }
    }
}