using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingTravelApi.Domains
{
    [Table("helpfuls")]
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
