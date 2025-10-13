using System.ComponentModel.DataAnnotations;

namespace BookingTravelApi.Domains
{
    public class Place
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        public int LocationId { get; set; }

        [Required]
        [MaxLength(255)]
        public String Name { get; set; } = null!;
    }
}
