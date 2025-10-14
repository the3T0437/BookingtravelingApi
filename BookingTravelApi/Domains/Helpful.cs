using System.ComponentModel.DataAnnotations;

namespace BookingTravelApi.Domains
{
    public class Helpful
    {
        [Required]
        [Key]
        public int UserId { get; set; }

        [Required]
        [Key]
        public int ReviewId { get; set; }

        public User? User { get; set; }
        public Review Review { get; set; }
    }
}
