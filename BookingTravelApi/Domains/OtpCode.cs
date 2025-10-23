using System.ComponentModel.DataAnnotations;

namespace BookingTravelApi.Domains
{
    public class OtpCode
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Email { get; set; }

        [Required]
        [MaxLength(50)]
        public string Code { get; set; }

        [Required]
        public DateTime ExpiryTime { get; set; }
    }
}
