using System.ComponentModel.DataAnnotations;
using BookingTravelApi.Domains;

namespace BookingTravelApi.DTO
{
    public class CreateOtpCodeDTO
    {
        [Required]
        [MaxLength(255)]
        public string Email { get; set; } = null!;

        public OtpCode Map()
        {
            var timeNow = DateTime.UtcNow.AddHours(7);

            //tạo mã
            Random random = new Random();
            var otp = random.Next(100000, 999999).ToString();


            return new OtpCode()
            {
                Email = Email,
                Code = otp,
                ExpiryTime = timeNow
            };
        }
    }
}