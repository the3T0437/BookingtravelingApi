using System.ComponentModel.DataAnnotations;

namespace BookingTravelApi.Domains
{
    public class Guide
    {
        [Required]
        [Key]
        public int StaffId { get; set; }

        [Required]
        [Key]
        public int ScheduleId { get; set; }
    }
}
