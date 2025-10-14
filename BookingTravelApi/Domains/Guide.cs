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

        public Staff? Staff { get; set; }
        public Schedule? Schedule { get; set; }
    }
}
