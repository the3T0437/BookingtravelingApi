using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingTravelApi.Domains
{
    [Table("places")]
    public class Place
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        public int LocationId { get; set; }

        [Required]
        [MaxLength(255)]
        public String Name { get; set; } = null!;

        public Location? Location { get; set; }
        public ICollection<LocationActivity>? LocationActivities { get; set; }
    }
}
