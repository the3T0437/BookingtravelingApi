using Microsoft.AspNetCore.Mvc;
using BookingTravelApi.Services;
using BookingTravelApi.DTO;
using BookingTravelApi.DTO.otpcode;
using BookingTravelApi.Domains;
using Microsoft.EntityFrameworkCore;

namespace BookingTravelApi.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class OTPController : ControllerBase
    {

        private MailService _mailService;
        private ApplicationDbContext _context;
        private readonly ILogger<LocationController> _logger;

        public OTPController(ILogger<LocationController> logger, ApplicationDbContext context, MailService mailService)
        {
            _context = context;
            _logger = logger;
            _mailService = mailService;
        }



        [HttpPost]
        [Route("send")]
        public async Task<IActionResult> SendOTP(CreateOtpCodeDTO createOtpCodeDTO)
        {

            if (string.IsNullOrEmpty(createOtpCodeDTO.Email))
            {
                return Problem("Email không được trống");
            }

            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == createOtpCodeDTO.Email);

            if (user == null)
            {
                return Problem("Email này không tồn tại");
            }

            var oldOtp = await _context.OtpCodes.AsNoTracking().FirstOrDefaultAsync(o => o.Email == createOtpCodeDTO.Email);

            if (oldOtp != null)
            {
                _context.OtpCodes.Remove(oldOtp);
            }

            var otpCode = createOtpCodeDTO.Map();

            _context.OtpCodes.Add(otpCode);
            await _context.SaveChangesAsync();

            var config = await _context.Configs.FindAsync(1);


            // gọi phương thức gửi mail
            bool success = await _mailService.SendMailAsync(
                otpCode.Email,
                otpCode.Code,
                config!.timeExpiredOtpSec
            );

            if (success)
            {
                return Ok(new RestDTO<bool>()
                {
                    Data = true
                });
            }
            else
            {
                return Problem("Gửi email thất bại");
            }
        }

        [HttpPost]
        [Route("verifyotp")]
        public async Task<IActionResult> VerifyOTP(OtpCodeDTO otpCodeDTO)
        {
            try
            {
                var timeNow = DateTime.Now;

                var otpCode = await _context.OtpCodes.AsNoTracking().FirstOrDefaultAsync(o => o.Email == otpCodeDTO.Email
                && o.Code == otpCodeDTO.Code
                && o.ExpiryTime > timeNow);

                if (otpCode == null)
                {
                    return NotFound("Không tìm thấy mã này");
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
