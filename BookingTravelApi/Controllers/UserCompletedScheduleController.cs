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

        [HttpGet("schedule/{scheduleId}")]
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

        [HttpGet("booking/{bookingId}")]
        public async Task<IActionResult> getBooking(int bookingId)
        {
            try
            {
                var query = await _context.UserCompletedSchedules
                .Where(b => b.BookingId == bookingId)
                .Include(u => u.Booking)
                .ThenInclude(b => b!.Schedule)
                .ThenInclude(t => t!.Tour)

                .Include(u => u.Booking)
                .ThenInclude(st => st!.Status)

                .Include(u => u.Booking)
                .ThenInclude(us => us!.User)
                .AsNoTracking().ToArrayAsync();

                var booking = query.Select(i => i.Map()).ToArray();
                return Ok(new RestDTO<UserCompletedScheduleDTO[]?>()
                {
                    Data = booking
                });
            }
            catch (IOException ex)
            {
                return Problem($"Get booking fail {ex.Message}");
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
                return Problem($"Error create: {ex.Message}");
            }

        }

        [HttpPut(Name = "updateUserCompletedSchedule")]
        public async Task<IActionResult> updateUserCompletedSchedule(UpdateUserCompletedScheduleDTO updateUserCompletedScheduleDTO)
        {
            try
            {
                var userCompletedSchedule = await _context.UserCompletedSchedules
                .Include(u => u.Booking)
                .ThenInclude(b => b.User)

                .Include(u => u.Booking)
                .ThenInclude(b => b.Actualcashs)

                .Include(u => u.Booking)
                .ThenInclude(b => b.Schedule)

                .Where(b => b.BookingId == updateUserCompletedScheduleDTO.BookingId).FirstOrDefaultAsync();

                if (userCompletedSchedule == null)
                {
                    return NotFound("userComplete not found");
                }

                var booking = userCompletedSchedule.Booking;
                var actualCash = booking.Actualcashs;
                var user = booking.User;
                var schedule = booking.Schedule;


                int oldActualCash = actualCash.money;

                userCompletedSchedule.countPeople = updateUserCompletedScheduleDTO.countPeople;


                int nonNumPeople = booking.NumPeople - updateUserCompletedScheduleDTO.countPeople;

                double percent = Convert.ToDouble(100 - schedule.Desposit) / 100.0;
                var tmp = Convert.ToInt32(schedule.FinalPrice * percent);


                if (nonNumPeople == 0)
                {
                    actualCash.money = updateUserCompletedScheduleDTO.countPeople * schedule.FinalPrice;
                }
                else if (nonNumPeople < booking.NumPeople)
                {
                    actualCash.money = booking.NumPeople * Convert.ToInt32(schedule.FinalPrice * schedule.Desposit / 100);
                    actualCash.money += updateUserCompletedScheduleDTO.countPeople * tmp;

                    // if(booking.ActualStatusId == Status.Paid)
                    // {
                    //     user.Money = nonNumPeople * tmp;
                    // }
                }
                else if (nonNumPeople == booking.NumPeople)
                {
                    actualCash.money = booking.NumPeople * Convert.ToInt32(schedule.FinalPrice * schedule.Desposit / 100);

                    // if(booking.ActualStatusId == Status.Paid)
                    // {
                    //     user.Money = nonNumPeople * tmp;
                    // }
                }

                int newActualCash = actualCash.money;
                int diff = newActualCash - oldActualCash;

                if (diff < 0)
                {
                    user.Money += Math.Abs(diff);
                }

                _context.UserCompletedSchedules.Update(userCompletedSchedule);
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