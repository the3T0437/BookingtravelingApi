using System.ComponentModel.DataAnnotations;

namespace BookingTravelApi.Domains
{
    public class LocationActivity
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        public int PlaceId { get; set; }

        [Required]
        [MaxLength(255)]
        public String Name { get; set; } = null!;

    }
}
