using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingTravelApi.Domains
{
    [Table("guides")]
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
