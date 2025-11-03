
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

                var query = await _context.Bookings
                .Where(b => b.UserId == userId)
                .Include(st => st.Status)
                .Include(us => us.User)

                .Include(s => s.Schedule)
                .ThenInclude(t => t!.Tour)
                .ThenInclude(d => d!.DayOfTours)

                .Include(s => s.Schedule)
                .ThenInclude(t => t!.Tour)
                .ThenInclude(tl => tl!.TourLocations)
                !.ThenInclude(l => l.Location)

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
                return Problem("Get booking fail");
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
                .Include(s => s.Status)
                .Include(s => s.User)

                .Include(s => s.Schedule)
                .ThenInclude(t => t!.Tour)
                .ThenInclude(t => t!.DayOfTours)

                .Include(s => s.Schedule)
                .ThenInclude(t => t!.Tour)
                .ThenInclude(t => t!.TourLocations)
                !.ThenInclude(t => t!.Location)

                .Include(s => s.Schedule)
                .ThenInclude(t => t!.Tour)
                .ThenInclude(t => t!.TourImages)
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

                var query = await _context.Bookings
                .Where(b => b.ScheduleId == scheduleId)
                .Include(s => s.Status)
                .Include(s => s.User)

                .Include(s => s.Schedule)
                .ThenInclude(t => t!.Tour)
                .ThenInclude(t => t!.DayOfTours)

                .Include(s => s.Schedule)
                .ThenInclude(t => t!.Tour)
                .ThenInclude(t => t!.TourLocations)
                !.ThenInclude(t => t!.Location)

                .Include(s => s.Schedule)
                .ThenInclude(t => t!.Tour)
                .ThenInclude(t => t!.TourImages)
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

                return Ok(new RestDTO<String>
                {
                    Data = $"Code: {booking.Code}"
                });
            }
            catch (Exception ex)
            {
                return Problem("create fail");
            }
        }

        [HttpPut(Name = "UpdateBooking")]
        public async Task<IActionResult> updateBooking(UpdateBookingDTO updateBooking)
        {
            try
            {
                if (updateBooking.CountChangeLeft > 3)
                {
                    return Problem("Enter less than 4 times");
                }
                
                var booking = await _context.Bookings.FirstOrDefaultAsync(s => s.Id == updateBooking.Id);
                if (booking == null)
                {
                    return NotFound($" ID {updateBooking.Id} not found.");
                }

                if(booking.CountChangeLeft >= 3)
                {
                    return Problem("Only changed three times");
                }

                updateBooking.UpdateEntity(booking);
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