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

        [HttpGet]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> getUsersCompletedSchedule(int? scheduleId = null)
        {
            try
            {
                if (scheduleId == null)
                {
                    return Problem("id not found");
                }

                var query = _context.UserCompletedSchedules.Include(u => u.Schedule)
                .ThenInclude(s => s!.Tour).ThenInclude(t => t!.TourLocations)
                !.ThenInclude(tl => tl!.Location)
                .Include(u => u.Schedule)
                .ThenInclude(s => s!.Tour)
                .ThenInclude(t => t!.TourImages)
                !.AsNoTracking();

                query = query.Where(g => g.ScheduleId == scheduleId);

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

        [HttpGet("User")]
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
                .Where(g => g.UserId == userId).Include(u => u.Schedule)
                .ThenInclude(s => s!.Tour).ThenInclude(t => t!.TourLocations)
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
                if (newUserSchedule == null)
                {
                    return NotFound("Successfully created the record, but failed to retrieve the full data for response.");
                }

                var userSchedule = newUserSchedule.Map();

                await _context.UserCompletedSchedules.AddAsync(userSchedule);
                await _context.SaveChangesAsync();

                
                var query = _context.UserCompletedSchedules
                .Where(g => g.UserId == userSchedule.UserId).Include(u => u.Schedule)
                .ThenInclude(s => s!.Tour).ThenInclude(t => t!.TourLocations)
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

                return Ok(new RestDTO<String>()
                {
                    Data = "200"
                });
            }
            catch (Exception ex)
            {
                return Problem("Error delete");
            }
        }

    }
}