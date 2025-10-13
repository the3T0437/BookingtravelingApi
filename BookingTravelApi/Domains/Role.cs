using System.ComponentModel.DataAnnotations;

namespace BookingTravelApi.Domains
{
    public class Role
    {
        [Required]
        [Key]
        public int id;

        [Required]
        [MaxLength(255)]
        public String Title { get; set; } = null!;

        public ICollection<User>? Users { get; set; }
    }
}
