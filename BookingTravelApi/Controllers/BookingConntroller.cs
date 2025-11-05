using BookingTravelApi.Domains;
using BookingTravelApi.DTO;
using BookingTravelApi.DTO.booking;
using BookingTravelApi.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingTravelApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BookingController : Controller
    {
        private ApplicationDbContext _context;
        private readonly ILogger<BookingController> _logger;
        public BookingController(ILogger<BookingController> logger, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("byUser/{userId}")]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> getBookingUserId(int? userId = null)
        {
            try
            {
                if (userId == null)
                {
                    return Problem("id not found");
                }
                var now = DateTime.Now;

                var query = await _context.Bookings
                .Where(b => b.UserId == userId && b.CreatedAt <= b.Schedule!.StartDate && b.Schedule!.StartDate > now)
                .Include(st => st.Status)
                .Include(us => us.User)

                .Include(s => s.Schedule)
                .ThenInclude(t => t!.Tour)
                .ThenInclude(ti => ti!.DayOfTours!)
                .ThenInclude(d => d.DayActivities!)
                .ThenInclude(da => da.Activity!)

                .Include(s => s.Schedule)
                .ThenInclude(t => t!.Tour)
                .ThenInclude(d => d!.DayOfTours)
                !.ThenInclude(d => d.DayActivities!)
                .ThenInclude(da => da.LocationActivity!)
                .ThenInclude(lo => lo.Place!)
                .ThenInclude(p => p.Location)

                .Include(s => s.Schedule)
                .ThenInclude(t => t!.Tour)
                .ThenInclude(i => i!.DayOfTours!)
                !.ThenInclude(i => i.DayActivities!)
                !.ThenInclude(i => i.LocationActivity)
                !.ThenInclude(i => i!.ActivityAndLocations)
                !.ThenInclude(i => i.Activity)

                .Include(s => s.Schedule)
                .ThenInclude(t => t!.Tour)
                .ThenInclude(tm => tm!.TourImages)
                .AsNoTracking().ToListAsync();

                var booking = query.Select(i => i.Map()).ToArray();

                return Ok(new RestDTO<BookingDTO[]?>()
                {
                    Data = booking
                });

            }
            catch(Exception ex)
            {
                return Problem($"Get booking fail {ex.Message}");
            }
        }

        [HttpGet("{bookingId}")]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> getBookingId(int? bookingId = null)
        {
            try
            {
                if (bookingId == null)
                {
                    return Problem("id not found");
                }

                var query = await _context.Bookings
                .Where(b => b.Id == bookingId)
                .Include(st => st.Status)
                .Include(us => us.User)

                .Include(s => s.Schedule)
                .ThenInclude(t => t!.Tour)
                .ThenInclude(ti => ti!.DayOfTours!)
                .ThenInclude(d => d.DayActivities!)
                .ThenInclude(da => da.Activity!)

                .Include(s => s.Schedule)
                .ThenInclude(t => t!.Tour)
                .ThenInclude(d => d!.DayOfTours)
                !.ThenInclude(d => d.DayActivities!)
                .ThenInclude(da => da.LocationActivity!)
                .ThenInclude(lo => lo.Place!)
                .ThenInclude(p => p.Location)

                .Include(s => s.Schedule)
                .ThenInclude(t => t!.Tour)
                .ThenInclude(i => i!.DayOfTours!)
                !.ThenInclude(i => i.DayActivities!)
                !.ThenInclude(i => i.LocationActivity)
                !.ThenInclude(i => i!.ActivityAndLocations)
                !.ThenInclude(i => i.Activity)

                .Include(s => s.Schedule)
                .ThenInclude(t => t!.Tour)
                .ThenInclude(tm => tm!.TourImages)
                .AsNoTracking().ToListAsync();

                var booking = query.Select(i => i.Map()).ToArray();
                return Ok(new RestDTO<BookingDTO[]?>()
                {
                    Data = booking
                });

            }
            catch(Exception ex)
            {
                return Problem("GetBooking faild");
            }
        }

        [HttpGet("bySchedule/{scheduleId}")]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> getBookingSchedule(int? scheduleId = null)
        {
            try
            {
                if (scheduleId == null)
                {
                    return Problem("id not found");
                }

                var now = DateTime.Now;

                var query = await _context.Bookings
                .Where(b => b.ScheduleId == scheduleId && b.CreatedAt <= b.Schedule!.StartDate && b.Schedule!.StartDate > now)
                .Include(st => st.Status)
                .Include(us => us.User)

                .Include(s => s.Schedule)
                .ThenInclude(t => t!.Tour)
                .ThenInclude(ti => ti!.DayOfTours!)
                .ThenInclude(d => d.DayActivities!)
                .ThenInclude(da => da.Activity!)

                .Include(s => s.Schedule)
                .ThenInclude(t => t!.Tour)
                .ThenInclude(d => d!.DayOfTours)
                !.ThenInclude(d => d.DayActivities!)
                .ThenInclude(da => da.LocationActivity!)
                .ThenInclude(lo => lo.Place!)
                .ThenInclude(p => p.Location)

                .Include(s => s.Schedule)
                .ThenInclude(t => t!.Tour)
                .ThenInclude(i => i!.DayOfTours!)
                !.ThenInclude(i => i.DayActivities!)
                !.ThenInclude(i => i.LocationActivity)
                !.ThenInclude(i => i!.ActivityAndLocations)
                !.ThenInclude(i => i.Activity)

                .Include(s => s.Schedule)
                .ThenInclude(t => t!.Tour)
                .ThenInclude(tm => tm!.TourImages)
                .AsNoTracking().ToListAsync();

                var booking = query.Select(i => i.Map()).ToArray();

                return Ok(new RestDTO<BookingDTO[]?>()
                {
                    Data = booking
                });

            }
            catch (Exception ex)
            {
                return Problem($"ERROR GuidesscheduleId");
            }
        }

        [HttpPost(Name = "CreateBooking")]
        public async Task<IActionResult> createBooking(CreateBookingDTO newBookingDTO)
        {
            try
            {
                var booking = newBookingDTO.Map();
                await _context.Bookings.AddAsync(booking);
                await _context.SaveChangesAsync();

                return Ok(new RestDTO<int>
                {
                    Data = booking.Id
                });
            }
            catch (Exception ex)
            {
                return Problem("create fail");
            }
        }

        [HttpPut("updateScheduleBooking")]
        public async Task<IActionResult> updateScheduleBooking(UpdateScheduleBookingDTO updateScheduleBooking)
        {
            try
            {
                // 1 trang thai xu ly 
                // 2 trang thai coc 
                // 3 trang thai thanh toan het 
                
                var booking = await _context.Bookings.FirstOrDefaultAsync(s => s.Id == updateScheduleBooking.Id);
                if (booking == null)
                {
                    return NotFound($" ID {updateScheduleBooking.Id} not found.");
                }

                if (booking.StatusId == 1)
                {
                    return Problem("Cannot change schedule while booking is processing.");
                }

                if (booking.CountChangeLeft <= 0)
                {
                    return Problem("Only changed three times");
                }

                var newSchedule = await _context.Schedules.FirstOrDefaultAsync(s => s.Id == updateScheduleBooking.ScheduleId);

                if ((newSchedule!.FinalPrice * booking.NumPeople) > booking.TotalPrice)
                {
                    booking.StatusId = 2;
                }
                else if((newSchedule!.FinalPrice * booking.NumPeople) <= booking.TotalPrice)
                {
                    booking.StatusId = 3;
                }

                booking.CountChangeLeft--;

                updateScheduleBooking.UpdateEntity(booking);
                await _context.SaveChangesAsync();

                return Ok(new RestDTO<Boolean>()
                {
                    Data = true
                });
            }
            catch (Exception ex)
            {
                return Problem($"Update fail {ex.Message}");
            }
        }

        [HttpPut("updateStatusBooking")]
        public async Task<IActionResult> updateStatusBooking(UpdateStatusBookingDTO updateStatusBooking)
        {
            try
            {
                var booking = await _context.Bookings.FirstOrDefaultAsync(s => s.Id == updateStatusBooking.Id);
                if (booking == null)
                {
                    return NotFound($" ID {updateStatusBooking.Id} not found.");
                }

                updateStatusBooking.UpdateEntity(booking);
                await _context.SaveChangesAsync();

                return Ok(new RestDTO<Boolean>()
                {
                    Data = true
                });
            }
            catch (Exception ex)
            {
                return Problem("Update fail");
            }
        }
        
        [HttpDelete("{bookingid}")]
        public async Task<IActionResult> deleteBooking(int bookingid)
        {
            try
            {
                var booking = await _context.Bookings.FindAsync(bookingid);
                if (booking == null)
                {
                    return Problem($"Id {bookingid} not found.");
                }

                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();

                return Ok(new RestDTO<Boolean>()
                {
                    Data = true
                });
            }
            catch(Exception ex)
            {
                return Problem("Delete Booking fail");
            }
        }

    }
}