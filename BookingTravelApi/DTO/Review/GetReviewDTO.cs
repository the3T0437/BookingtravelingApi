using System.ComponentModel.DataAnnotations;
using BookingTravelApi.Domains;

namespace BookingTravelApi.DTO.review
{
    public class GetReviewDTO
    {
        public int? UserId { get; set; }

        [Required]
        public int TourId { get; set; }
    }
}