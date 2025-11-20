using System.Linq.Dynamic.Core;
using BookingTravelApi.Domains;
using BookingTravelApi.DTO;
using BookingTravelApi.DTO.Activity;
using BookingTravelApi.DTO.booking;
using BookingTravelApi.DTO.schedule;
using BookingTravelApi.DTO.status;
using BookingTravelApi.DTO.Tour;
using BookingTravelApi.DTO.user;
using BookingTravelApi.DTO.usercompletedschedule;
using BookingTravelApi.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingTravelApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserCompletedScheduleController : Controller
    {
        private ApplicationDbContext _context;
        private readonly ILogger<UserCompletedScheduleController> _logger;

        public UserCompletedScheduleController(ILogger<UserCompletedScheduleController> logger, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("{scheduleId}")]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> GetCompletedBookings(int scheduleId)
        {
            try
            {
                var bookings = await _context.UserCompletedSchedules
                .Where(u => u.Booking!.ScheduleId == scheduleId)
                .Include(u => u.Booking)
                .ThenInclude(b => b!.Schedule)
                .ThenInclude(t => t!.Tour)
                .Include(u => u.Booking)
                .ThenInclude(b => b!.User)
                .Include(u => u.Booking)
                .ThenInclude(b => b!.Status)
                .AsNoTracking()
                .ToListAsync();

                var comp = bookings.Select(i => i.Map()).ToArray();

                return Ok(new RestDTO<UserCompletedScheduleDTO[]?>
                {
                    Data = comp
                });
            }
            catch (Exception ex)
            {
                return Problem($"Error get completed bookings: {ex.Message}");
            }
        }


        [HttpPost]
        public async Task<IActionResult> createUserCompletedSchedule(CreateUserCompletedScheduleDTO newUserSchedule)
        {
            try
            {
                var userSchedule = newUserSchedule.Map();
               
                await _context.UserCompletedSchedules.AddAsync(userSchedule);
                await _context.SaveChangesAsync();

                return Ok(new RestDTO<int>()
                {
                    Data = userSchedule.BookingId
                });
            }
            catch (Exception ex)
            {
                return Problem("Error create");
            }

        }

        [HttpPut(Name = "updateUserCompletedSchedule")]
        public async Task<IActionResult> updateUserCompletedSchedule(UpdateUserCompletedScheduleDTO updateBooking)
        {
            try
            {
                var query = await _context.UserCompletedSchedules.Where(b => b.BookingId == updateBooking.BookingId).FirstOrDefaultAsync();
                if (query == null)
                {
                    return Problem("Update fail");
                }

                query.countPeople = updateBooking.countPeople;

                _context.UserCompletedSchedules.Update(query);
                await _context.SaveChangesAsync();

                return Ok(new RestDTO<Boolean>()
                {
                    Data = true
                });
            }
            catch (IOException ex)
            {
                return Problem($"Update fail {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> deleteUserCompletedSchedule(int id)
        {
            try
            {
                var userSchedule = await _context.UserCompletedSchedules
                .Where(g => g.BookingId == id)
                .FirstOrDefaultAsync();

                if (userSchedule == null)
                {
                    return NotFound($"Place with Id {id} not found.");
                }

                _context.UserCompletedSchedules.Remove(userSchedule);
                await _context.SaveChangesAsync();

                return Ok(new RestDTO<Boolean>()
                {
                    Data = true
                });
            }
            catch (Exception ex)
            {
                return Problem($"Error delete {ex.Message}");
            }
        }
    }
}