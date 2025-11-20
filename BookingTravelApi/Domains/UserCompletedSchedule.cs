using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingTravelApi.Domains
{
    [Table("usercompletedschedules")]
    public class UserCompletedSchedule
    {
        [Required]
        [Key]
        public int BookingId { get; set; }

        [Required]
        public int countPeople { get; set; }

        public int UserId { get; set; }
        public int ScheduleId { get; set; }

        public Booking? Booking { get; set; }
    }
}
