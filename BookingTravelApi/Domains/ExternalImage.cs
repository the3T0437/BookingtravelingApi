using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingTravelApi.Domains
{
    [Table("externalImages")]
    public class ExternalImage
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public String Path { get; set; } = null!;
    }
}
