
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
            catch (Exception ex)
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

                var booking = query.Select(i => i.Map()).FirstOrDefault();

                return Ok(new RestDTO<BookingDTO?>()
                {
                    Data = booking
                });

            }
            catch (Exception ex)
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
        public async Task<IActionResult> UpdateBooking(UpdateBookingDTO updateBooking)
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

                if (booking.CountChangeLeft >= 3)
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

        [HttpDelete("{bookingId}")]
        public async Task<IActionResult> DeleteBooking(int bookingId)
        {
            try
            {
                var booking = await _context.Bookings
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
                .Where(b => b.Id == bookingId)
                .FirstOrDefaultAsync();

                if (booking == null)
                {
                    return Problem($"Id {bookingId} not found.");
                }

                var deposit = booking.Schedule.FinalPrice * booking.Schedule.Desposit / 100 * booking.NumPeople;
                var moneyLeft = booking.TotalPrice - deposit;
                moneyLeft = moneyLeft > 0 ? moneyLeft : 0;

                var user = await _context.Users.FindAsync(booking.UserId);
                user!.Money += moneyLeft;
                _context.Users.Update(user);

                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();

                return Ok(new RestDTO<BookingDTO>()
                {
                    Data = booking.Map()
                });
            }
            catch (Exception ex)
            {
                return Problem("Delete Booking fail");
            }
        }

        [HttpPost("changeBooking")]
        public async Task<IActionResult> ChangeBooking(ChangeBookingDTO changeBooking)
        {
            var booking = await _context.Bookings.Include(i => i.Schedule).Where(i => i.Id == changeBooking.BookingId).FirstOrDefaultAsync();

            if (booking == null)
            {
                return NotFound(new ErrorDTO("Không tìm thấy hóa đơn tương ứng"));
            }

            if (booking.Schedule!.StartDate.AddDays(-3) < DateTime.Now)
            {
                return BadRequest(new ErrorDTO("Đã quá ngày để có thể đổi"));
            }

            if (booking.CountChangeLeft - 1 < 0)
            {
                return BadRequest(new ErrorDTO("Đã đổi quá số lần"));
            }

            UpdateBookingDTO updateBookingDTO = new UpdateBookingDTO()
            {
                Id = booking.Id,
                CountChangeLeft = booking.CountChangeLeft - 1,
                ScheduleId = changeBooking.ScheduleId,
                StatusId = booking.StatusId
            };

            //TODO: update ví tiền
            await UpdateBooking(updateBookingDTO);

            return await getBookingId(changeBooking.BookingId);
        }
    }
}