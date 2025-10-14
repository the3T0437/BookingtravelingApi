using System.ComponentModel.DataAnnotations;

namespace BookingTravelApi.Domains
{
    public class Location
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public String Name { get; set; } = null!;

        public ICollection<TourLocation>? TourLocations { get; set; }
        public ICollection<Place>? Places { get; set; }
    }
}
