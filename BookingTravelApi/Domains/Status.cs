using System.ComponentModel.DataAnnotations;

namespace BookingTravelApi.Domains
{
    public class Status
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        public ICollection<Booking>? Bookings { get; set; }
    }
}
