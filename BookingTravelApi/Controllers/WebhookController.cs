using System.Linq.Dynamic.Core;
using BookingTravelApi.Domains;
using BookingTravelApi.DTO;
using BookingTravelApi.DTO.ChangePassword;
using BookingTravelApi.DTO.checkAccount;
using BookingTravelApi.DTO.loginDTO;
using BookingTravelApi.DTO.loginEmailDTO;
using BookingTravelApi.DTO.updatePassword;
using BookingTravelApi.DTO.user;
using BookingTravelApi.Extensions;
using BookingTravelApi.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PayOS;
using PayOS.Models.Webhooks;

namespace BookingTravelApi.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class WebhookController : Controller
    {
        private PayOSClient _payOs;
        private ApplicationDbContext _context;
        private readonly ILogger<WebhookController> _logger;

        public WebhookController(ILogger<WebhookController> logger, ApplicationDbContext context, PayOSClient payOs)
        {
            _context = context;
            _logger = logger;
            _payOs = payOs;
        }

        [HttpPost("payment")]
        public async Task<IActionResult> VerifyPayment(Webhook webhook)
        {
            if (webhook == null)
            {
                return BadRequest("Webhook data is required");
            }

            try
            {
                var webhookData = await _payOs.Webhooks.VerifyAsync(webhook);

                var booking = await _context.Bookings
                    .Include(i => i.Schedule)
                    .Include(i => i.User)
                    .Where(i => i.Id == webhookData.OrderCode).FirstOrDefaultAsync();
                if (booking == null)
                {
                    return Ok();
                }

                if (DateTime.UtcNow.AddHours(7) > booking.ExpiredAt)
                {
                    booking.User!.Money += booking.TotalPrice;
                    await WriteSomeFile($"expired {booking.Id}");
                }
                else if (booking.TotalPrice == booking.Schedule.FinalPrice * booking.NumPeople)
                {
                    booking.StatusId = Status.Paid;
                    await WriteSomeFile($"paid {booking.Id}");
                }
                else
                {
                    booking.StatusId = Status.Deposit;
                    await WriteSomeFile($"deposit {booking.Id}");
                }

                await _context.SaveChangesAsync();
                await WriteSomeFile("done");
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Webhook processing error: {ex.Message}");
                await WriteSomeFile(ex.Message);
                return Problem(ex.Message);
            }
        }

        private async Task WriteSomeFile(String content)
        {

            // Define the path where you want to save the image
            var uploadsFolder = AppConfig.GetImagePath();
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Generate a unique file name to avoid conflicts
            var uniqueFileName = "test.txt";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Save the image to the specified path
            using (var stream = new StreamWriter(filePath, true))
            {
                stream.Write("content \n");
            }
        }
    }
}