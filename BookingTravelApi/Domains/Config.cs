using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingTravelApi.Domains
{
    [Table("configs")]
    public class Configs
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int countChangeSchedule {get; set;}

        [Required]
        public int timeExpiredBookingSec {get; set;}

        [Required]
        public int timeExpiredOtpSec {get; set;}
    }
}
