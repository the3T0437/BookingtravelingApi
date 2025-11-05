using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingTravelApi.Domains
{
    [Table("activities")]
    public class Activity
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public String Action { get; set; } = null!;

        public ICollection<DayActivity>? DayActivities { get; set; }
        public ICollection<ActivityAndLocation>? ActivityAndLocations { get; set; }
    }
}
