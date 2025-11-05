using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingTravelApi.Domains
{
    [Table("tourimages")]
    public class TourImage
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        public int TourId { get; set; }

        [Required]
        [MaxLength(255)]
        public String Path { get; set; } = null!;

        public Tour? Tour { get; set; }
    }
}
