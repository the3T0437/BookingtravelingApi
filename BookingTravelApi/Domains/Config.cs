using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingTravelApi.Domains
{
    [Table("configs")]
    public class Configs
    {
        [Key]
        [Required]
        public int Id  { get; set; }

        [MaxLength(255)]
        public String Name {get; set;} = null!;

        [Required]
        public int Value {get; set;}    
    }
}
