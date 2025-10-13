using System.ComponentModel.DataAnnotations;

namespace BookingTravelApi.Domains
{
    public class ActivityAtLocationActivity
    {
        [Required]
        [Key]
        public int ActivityId { get; set; }

        [Required]
        [Key]
        public int LocationActivityId { get; set; }
    }
}
