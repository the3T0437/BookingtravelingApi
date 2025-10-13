using System.ComponentModel.DataAnnotations;

namespace BookingTravelApi.Domains
{
    public class Activity
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public String Action { get; set; } = null!;
    }
}
