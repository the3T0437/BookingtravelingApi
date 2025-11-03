using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingTravelApi.Domains
{
    [Table("activityandlocations")]
    public class ActivityAndLocation
    {
        [Required]
        [Key]
        public int ActivityId { get; set; }

        [Required]
        [Key]
        public int LocationActivityId { get; set; }

        public Activity? Activity { get; set; }
        public LocationActivity? LocationActivity { get; set; }
    }
}
