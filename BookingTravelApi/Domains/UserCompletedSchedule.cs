using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingTravelApi.Domains
{
    [Table("usercompletedschedules")]
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
