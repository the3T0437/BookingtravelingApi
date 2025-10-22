using Microsoft.AspNetCore.Mvc;
using BookingTravelApi.Services;
using BookingTravelApi.DTO.SendMail;
using BookingTravelApi.DTO;

namespace BookingTravelApi.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {

        private MailService _mailService;

        public MailController(MailService mailService)
        {
            _mailService = mailService;
        }

        [HttpPost]
        [Route("send")]
        public async Task<IActionResult> SendMail(SendMailDTO sendMailDTO)
        {
            if (string.IsNullOrEmpty(sendMailDTO.toEmail))
            {
                return Problem("Email không được trống");
            }

            //tạo mã
            Random random = new Random();
            var emailBody = random.Next(100000, 999999).ToString();

            // gọi phương thức gửi mail
            bool success = await _mailService.SendMailAsync(
                sendMailDTO.toEmail,
                emailBody
            );

            if (success)
            {
                return Ok(new RestDTO<string>()
                {
                    Data = emailBody
                });
            }
            else
            {
                return Problem("Gửi email thất bại");
            }
        }
    }
}
