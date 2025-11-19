using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingTravelApi.Domains
{
    [Table("actualcashs")]
    public class Actualcashs
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int money {get; set;}
        
        [Required]
        public DateTime CreatedAt { get; set; }
    }
}