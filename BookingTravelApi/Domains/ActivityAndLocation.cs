using System.ComponentModel.DataAnnotations;

namespace BookingTravelApi.Domains
{
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
