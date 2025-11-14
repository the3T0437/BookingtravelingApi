using System.ComponentModel.DataAnnotations;
using BookingTravelApi.Domains;

namespace BookingTravelApi.DTO.Helpful
{
    public class HelpfulDTO
    {
        [Required]
        public bool isEnable { get; set; }
        [Required]
        public int UserId { get; set; }

        [Required]
        public int ReviewId { get; set; }
    }
}
