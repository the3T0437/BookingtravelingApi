using System.ComponentModel.DataAnnotations;

namespace BookingTravelApi.Domains
{
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
        public DateTime time { get; set; }
    }
}
