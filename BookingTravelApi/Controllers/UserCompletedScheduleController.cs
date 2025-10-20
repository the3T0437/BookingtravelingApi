using System.Linq.Dynamic.Core;
using BookingTravelApi.Domains;
using BookingTravelApi.DTO;
using BookingTravelApi.DTO.Activity;
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

        [HttpGet("ByScheduleId")]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> getUsersCompletedSchedule(int? scheduleId = null)
        {
            try
            {
                if (scheduleId == null)
                {
                    return Problem("id not found");
                }

                var query = _context.UserCompletedSchedules
                .Where(g => g.ScheduleId == scheduleId)
                .Include(u => u.Schedule)
                .ThenInclude(s => s!.Tour)
                .ThenInclude(t => t!.TourLocations)
                !.ThenInclude(tl => tl!.Location)

                .Include(u => u.Schedule)
                .ThenInclude(s => s!.Tour)
                .ThenInclude(t => t!.TourImages)
                !.AsNoTracking();

                var userScheduleDTO = await query.Select(i => i.Map()).ToArrayAsync();

                return Ok(new RestDTO<UserCompletedScheduleDTO[]?>()
                {
                    Data = userScheduleDTO
                });
            }
            catch (Exception ex)
            {
                return Problem("Error get ScheduleCompleted");
            }
        }

        [HttpGet("ByUser")]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> getScheduleCompleted(int? userId = null)
        {
            try
            {
                if (userId == null)
                {
                    return Problem("id not found");
                }

                var query = _context.UserCompletedSchedules
                .Where(g => g.UserId == userId)
                .Include(u => u.Schedule)
                .ThenInclude(s => s!.Tour)
                .ThenInclude(t => t!.TourLocations)
                !.ThenInclude(tl => tl.Location)

                .Include(u => u.Schedule)
                .ThenInclude(s => s!.Tour)
                .ThenInclude(t => t!.TourImages)!
                .AsNoTracking();

                var userScheduleDTO = await query.Select(i => i.Map()).ToArrayAsync();

                return Ok(new RestDTO<UserCompletedScheduleDTO[]?>()
                {
                    Data = userScheduleDTO
                });
            }
            catch (Exception ex)
            {
                return Problem("Error get ScheduleCompleted");
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
                    Data = userSchedule.UserId
                });
            }
            catch (Exception ex)
            {
                return Problem("Error create");
            }

        }

        [HttpDelete(Name = "DeleteUserCompletedSchedule")]
        public async Task<IActionResult> deleteUserCompletedSchedule(int userId, int scheduleId)
        {
            try
            {
                var userSchedule = await _context.UserCompletedSchedules
                .Where(g => g.UserId == userId && g.ScheduleId == scheduleId)
                .FirstOrDefaultAsync();

                if (userSchedule == null)
                {
                    return NotFound($"Place with Id {userId} or {scheduleId} not found.");
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
                return Problem("Error delete");
            }
        }

    }
}