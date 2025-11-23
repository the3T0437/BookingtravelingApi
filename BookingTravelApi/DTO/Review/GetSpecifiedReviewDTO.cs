using System.ComponentModel.DataAnnotations;
using BookingTravelApi.Domains;

namespace BookingTravelApi.DTO.review
{
    public class getSpecifiedReviewDTO
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int ScheduleId { get; set; }
    }
}