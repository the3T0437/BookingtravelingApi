using System.ComponentModel.DataAnnotations;

namespace BookingTravelApi.DTO.config
{
    public class ConfigDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int countChangeSchedule { get; set; }
        [Required]
        public int timeExpiredBookingSec { get; set; }
        [Required]
        public int timeExpiredOtpSec { get; set; }

    }
}