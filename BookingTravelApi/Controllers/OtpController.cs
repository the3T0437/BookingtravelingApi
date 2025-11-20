using Microsoft.AspNetCore.Mvc;
using BookingTravelApi.Services;
using BookingTravelApi.DTO;
using BookingTravelApi.DTO.otpcode;
using BookingTravelApi.Domains;
using Microsoft.EntityFrameworkCore;
using BookingTravelApi.Helpers;

namespace BookingTravelApi.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class OTPController : ControllerBase
    {

        private MailService _mailService;
        private ApplicationDbContext _context;
        private readonly ILogger<OTPController> _logger;

        public OTPController(ILogger<OTPController> logger, ApplicationDbContext context, MailService mailService)
        {
            _context = context;
            _logger = logger;
            _mailService = mailService;
        }



        [HttpPost]
        [Route("send")]
        public async Task<IActionResult> SendOTP(CreateOtpCodeDTO createOtpCodeDTO)
        {
            try
            {
                if (string.IsNullOrEmpty(createOtpCodeDTO.Email))
                {
                    return Problem("Email không được trống");
                }


                var oldOtp = await _context.OtpCodes.AsNoTracking().FirstOrDefaultAsync(o => o.Email == createOtpCodeDTO.Email);

                if (oldOtp != null)
                {
                    _context.OtpCodes.Remove(oldOtp);
                }

                var config = await _context.Configs.FindAsync(3);

                DateTime now = DateTimeHelper.GetVietNamTime();
                var time = now.AddMinutes(config!.Value);

                var otpCode = createOtpCodeDTO.Map();
                otpCode.ExpiryTime = time;

                _context.OtpCodes.Add(otpCode);
                await _context.SaveChangesAsync();


                // gọi phương thức gửi mail
                SendOtpStatus success = await _mailService.SendMailAsync(
                    otpCode.Email,
                    otpCode.Code,
                    config!.Value
                );

                Console.WriteLine($"Result: {success.Status} - {success.Message}");

                if (success.Status)
                {
                    return Ok(new RestDTO<bool>()
                    {
                        Data = true
                    });
                }
                else
                {
                    return Problem($"{success.Message}");
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    error = ex.Message,
                    type = ex.GetType().Name,
                    innerError = ex.InnerException?.Message,
                    stackTrace = ex.StackTrace
                });
            }

        }

        [HttpPost]
        [Route("verifyotp")]
        public async Task<IActionResult> VerifyOTP(OtpCodeDTO otpCodeDTO)
        {
            try
            {
                var timeNow = DateTimeHelper.GetVietNamTime();

                var otpCode = await _context.OtpCodes.AsNoTracking().FirstOrDefaultAsync(o => o.Email == otpCodeDTO.Email
                && o.Code == otpCodeDTO.Code);

                if (otpCode == null)
                {
                    return Problem("Không tìm thấy mã này");
                }

                if (otpCode.ExpiryTime < timeNow)
                {
                    return Problem("Mã đã hết hạn");
                }


                return Ok(new RestDTO<bool>()
                {
                    Data = true
                });
            }
            catch (Exception ex)
            {
                return Problem("Lỗi" + ex);
            }
        }
    }
}
