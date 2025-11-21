using System.ComponentModel.DataAnnotations;
using BookingTravelApi.Domains;
using BookingTravelApi.Helpers;

namespace BookingTravelApi.DTO
{
    public class CreateOtpCodeDTO
    {
        [Required]
        [MaxLength(255)]
        public string Email { get; set; } = null!;

        public OtpCode Map()
        {
            var timeNow = DateTimeHelper.GetVietNamTime();

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