using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingTravelApi.Domains
{
    [Table("banks")]
    public class Bank
    {
        [Required]
        [Key]
        public int id { get; set; }

        [Required]
        public String name { get; set; } = null!;

    }
}
