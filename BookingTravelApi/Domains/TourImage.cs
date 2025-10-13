using System.ComponentModel.DataAnnotations;

namespace BookingTravelApi.Domains
{
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
