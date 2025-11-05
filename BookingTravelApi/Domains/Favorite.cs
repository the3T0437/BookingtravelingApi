using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingTravelApi.Domains
{
    [Table("favorites")]
    public class Favorite
    {
        [Required]
        [Key]
        public int UserId { get; set; }

        [Required]
        [Key]
        public int TourId { get; set; }

        public User? User { get; set; }
        public Tour? Tour { get; set; }
    }
}
