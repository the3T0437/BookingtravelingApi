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

<<<<<<< HEAD
        [HttpGet("{scheduleId}")]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> getUsersCompletedSchedule(int? scheduleId = null)
        {
            try
            {
                if (scheduleId == null)
                {
                    return Problem("id not found");
                }

                var query = await _context.UserCompletedSchedules
                .Where(g => g.Booking!.ScheduleId == scheduleId)
                .Include(u => u.Booking)
                .ThenInclude(s => s!.Schedule)
                .AsNoTracking().ToListAsync();

                var userScheduleDTO = query.Select(i => i.Map()).ToArray();

                return Ok(new RestDTO<UserCompletedScheduleDTO[]?>()
                {
                    Data = userScheduleDTO
                });
            }
            catch (Exception ex)
            {
                return Problem($"Error get ScheduleCompleted {ex.Message}");
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
                return Problem($"Error create {ex.Message}");
            }

        }

        [HttpPut("{bookingId}")]
        public async Task<IActionResult> updateUserCompletedSchedule(UpdateUserCompletedScheduleDTO updateBooking)
        {
            try
            {
                var query = await _context.UserCompletedSchedules.Where(b => b.BookingId == updateBooking.BookingId).FirstOrDefaultAsync();
                if(query == null)
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
            catch(IOException ex)
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
=======
        // [HttpGet("{scheduleId}")]
        // [ResponseCache(NoStore = true)]
        // public async Task<IActionResult> getUsersCompletedSchedule(int? scheduleId = null)
        // {
        //     try
        //     {
        //         if (scheduleId == null)
        //         {
        //             return Problem("id not found");
        //         }

        //         var query = _context.UserCompletedSchedules
        //         .Where(g => g.ScheduleId == scheduleId)
        //         .Include(u => u.User)

        //         .Include(u => u.Schedule)
        //         .ThenInclude(s => s!.Bookings)
        //         .AsNoTracking();

        //         var userScheduleDTO = await query.Select(i => i.Map()).ToArrayAsync();

        //         return Ok(new RestDTO<UserCompletedScheduleDTO[]?>()
        //         {
        //             Data = userScheduleDTO
        //         });
        //     }
        //     catch (Exception ex)
        //     {
        //         return Problem("Error get ScheduleCompleted");
        //     }
        // }

        [HttpPost]
        public async Task<IActionResult> createUserCompletedSchedule(CreateUserCompletedScheduleDTO newUserSchedule)
        {
            try
            {
                var booking = await _context.Bookings.AsNoTracking().FirstOrDefaultAsync(b => b.Id == newUserSchedule.BookingId);

                var userSchedule = newUserSchedule.Map();
                userSchedule.ScheduleId = booking!.ScheduleId;
                userSchedule.UserId = booking!.UserId;

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

        // [HttpDelete(Name = "DeleteUserCompletedSchedule")]
        // public async Task<IActionResult> deleteUserCompletedSchedule(int userId, int scheduleId)
        // {
        //     try
        //     {
        //         var userSchedule = await _context.UserCompletedSchedules
        //         .Where(g => g.UserId == userId && g.ScheduleId == scheduleId)
        //         .FirstOrDefaultAsync();

        //         if (userSchedule == null)
        //         {
        //             return NotFound($"Place with Id {userId} or {scheduleId} not found.");
        //         }

        //         _context.UserCompletedSchedules.Remove(userSchedule);
        //         await _context.SaveChangesAsync();

        //         return Ok(new RestDTO<Boolean>()
        //         {
        //             Data = true
        //         });
        //     }
        //     catch (Exception ex)
        //     {
        //         return Problem("Error delete");
        //     }
        // }
>>>>>>> main

    }
}