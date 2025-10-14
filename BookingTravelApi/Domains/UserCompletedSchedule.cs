using System.ComponentModel.DataAnnotations;

namespace BookingTravelApi.Domains
{
    public class UserCompletedSchedule
    {
        [Required]
        [Key]
        public int UserId { get; set; }

        [Required]
        [Key]
        public int ScheduleId { get; set; }

        public User? User { get; set; }
        public Schedule? Schedule { get; set; }
    }
}
