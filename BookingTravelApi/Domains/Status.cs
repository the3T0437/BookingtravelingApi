using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingTravelApi.Domains
{

    [Table("status")]
    public class Status
    {
        public static int Processing = 1;
        public static int Deposit = 2;
        public static int Paid = 3;

        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        public ICollection<Booking>? Bookings { get; set; }
    }
}
