using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.ConstrainedExecution;

namespace BookingTravelApi.Domains
{
    [Table("configs")]
    public class Configs
    {
        public static readonly int CountChangeSchedule = 1;
        public static readonly int TimeExpiredBookingHour = 2;
        public static readonly int TimeExpiredOtpMinutes = 3;

        [Key]
        [Required]
        public int Id  { get; set; }

        [MaxLength(255)]
        public String Name {get; set;} = null!;

        [Required]
        public int Value {get; set;}    
    }
}
