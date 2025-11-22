using BookingTravelApi.Domains;
using BookingTravelApi.DTO.notification;
using BookingTravelApi.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingTravelApi.DTO.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class NotificationController : Controller
    {
        private ApplicationDbContext _context;
        private readonly ILogger<NotificationController> _logger;
        private readonly FirebaseNotificationService _notificationService;
        public NotificationController(ILogger<NotificationController> logger, ApplicationDbContext context, FirebaseNotificationService notificationService)
        {
            _context = context;
            _logger = logger;
            _notificationService = notificationService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendNotification([FromBody] PushNotificationRequest request)
        {
            try
            {
                var response = await _notificationService.SendNotification(
                    request.Token,
                    request.Title,
                    request.Body
                );

                return Ok(new RestDTO<bool>
                {
                    Data = true
                });
            }
            catch (Exception ex)
            {
                return Problem($"Lỗi {ex.Message}");
            }
        }

        [HttpPost("send-multiple")]
        public async Task<IActionResult> SendMultipleNotifications([FromBody] MultipleNotificationRequest request)
        {
            try
            {
                var response = await _notificationService.SendMulticastNotification(
                    request.Tokens,
                    request.Title,
                    request.Body
                );

                return Ok(new RestDTO<bool>
                {
                    Data = true
                });
            }
            catch (Exception ex)
            {
                return Problem($"Lỗi {ex.Message}");
            }
        }

        [HttpGet("{userId}")]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> getNotification(int? userId = null)
        {
            try
            {
                if (userId == null)
                {
                    return Problem("id not found");
                }

                var query = await _context.Notifications
                .Where(n => n.UserId == userId)
                .AsNoTracking().ToListAsync();

                var notification = query.Select(i => i.Map()).ToArray();

                return Ok(new RestDTO<NotificationDTO[]?>()
                {
                    Data = notification
                });
            }
            catch (Exception ex)
            {
                return Problem("Get Notification fail");
            }
        }

        [HttpPost(Name = "CreateNotification")]
        public async Task<IActionResult> createNotification(CreateNotificationDTO newNotification)
        {
            try
            {
                var notification = newNotification.Map();
                await _context.Notifications.AddAsync(notification);
                await _context.SaveChangesAsync();

                return Ok(new RestDTO<int>()
                {
                    Data = notification.Id
                });
            }
            catch (Exception ex)
            {
                return Problem("Create Notification fail");
            }
        }

        [HttpPut(Name = "UpdateNotification")]
        public async Task<IActionResult> updateNotification(UpdateNotificationDTO updateNotification)
        {
            try
            {
                var notification = await _context.Notifications.FirstOrDefaultAsync(s => s.Id == updateNotification.Id);

                if (notification == null)
                {
                    return Problem($"ID {updateNotification.Id} not found.");
                }

                updateNotification.UpdateEntity(notification);
                await _context.SaveChangesAsync();

                return Ok(new RestDTO<Boolean>()
                {
                    Data = true
                });
            }
            catch (Exception ex)
            {
                return Problem("Update Notification fail");
            }
        }

        [HttpDelete("{notificationId}")]
        public async Task<IActionResult> deleteNotification(int id)
        {
            try
            {
                var notification = await _context.Notifications.FindAsync(id);
                if (notification == null)
                {
                    return Problem($"ID {id} not found");
                }

                _context.Notifications.Remove(notification);
                await _context.SaveChangesAsync();

                return Ok(new RestDTO<Boolean>()
                {
                    Data = true
                });
            }
            catch (Exception ex)
            {
                return Problem("Delete Notification fail");
            }
        }

    }
}