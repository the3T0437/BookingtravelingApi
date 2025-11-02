using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingTravelApi.Domains
{
    [Table("dayactivities")]
    public class DayActivity
    {
        [Required]
        [Key]
        public int DayOfTourId { get; set; }

        [Required]
        [Key]
        public int ActivityId { get; set; }

        [Required]
        [Key]
        public int LocationActivityId { get; set; }

        [Required]
        public TimeSpan Time { get; set; }

        public DayOfTour? DayOfTour { get; set; }
        public Activity? Activity { get; set; }
        public LocationActivity? LocationActivity { get; set; }
    }
}
