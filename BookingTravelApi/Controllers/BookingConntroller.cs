using System.Runtime.CompilerServices;
using BookingTravelApi.Domains;
using BookingTravelApi.DTO;
using BookingTravelApi.DTO.booking;
using BookingTravelApi.Extensions;
using BookingTravelApi.Helpers;
using BookingTravelApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PayOS.Models.V2.PaymentRequests;

namespace BookingTravelApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BookingController : Controller
    {
        private ApplicationDbContext _context;
        private PaymentService _paymentService;
        private readonly ILogger<BookingController> _logger;
        public BookingController(ILogger<BookingController> logger, ApplicationDbContext context, PaymentService paymentService)
        {
            _context = context;
            _logger = logger;
            _paymentService = paymentService;
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
                var now = DateTimeHelper.GetVietNamTime();

                var query = await _context.Bookings
                .Where(b => b.UserId == userId && b.Schedule!.StartDate > now)
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
                .AsNoTracking().FirstAsync();

                var booking = query.Map();

                if (booking.TotalPrice / booking.NumPeople == booking.Schedule.FinalPrice)
                {
                    booking.PayType = true;
                }
                return Ok(new RestDTO<BookingDTO>()
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

                var now = DateTimeHelper.GetVietNamTime();

                var query = await _context.Bookings
                .Where(b => b.ScheduleId == scheduleId && b.Schedule!.StartDate > now)
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
            using var transient = await _context.Database.BeginTransactionAsync();

            try
            {
                var schedule = await _context.Schedules.FindAsync(newBookingDTO.ScheduleId);
                if (schedule == null) return Problem("scheduleId not Found");
                var config = await _context.Configs.AsNoTracking().FirstOrDefaultAsync(c => c.Id == 1);

                var booking = newBookingDTO.Map();
                booking.CountChangeLeft = config!.countChangeSchedule;

                await _context.Bookings.AddAsync(booking);
                await _context.SaveChangesAsync();

                booking.Code = $"{schedule.Code}-{booking.Id}";
                _context.Bookings.Update(booking);
                await _context.SaveChangesAsync();

                return Ok(new RestDTO<int>
                {
                    Data = booking.Id
                });
            }
            catch (Exception ex)
            {
                await transient.RollbackAsync();
                return Problem($"create fail {ex.Message}");
            }
        }

        private async Task createPaymentLink(Booking booking, DateTime expiredAt)
        {
            PaymentLinkItem item = new PaymentLinkItem()
            {
                Name = $"booking ${booking.Id}",
                Quantity = 1,
                Price = booking.TotalPrice
            };
            
            var response = await _paymentService.createPayment(booking.Id, [item], expiredAt);
            booking.Qr = response.QrCode;
            booking.ExpiredAt = expiredAt;
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
                    return BadRequest("Cannot change schedule while booking is processing.");
                }

                if (booking.CountChangeLeft <= 0)
                {
                    return BadRequest("Only changed three times");
                }

                var newSchedule = await _context.Schedules.FirstOrDefaultAsync(s => s.Id == updateScheduleBooking.ScheduleId);

                if (newSchedule == null)
                {
                    return NotFound($"Schedule ID {updateScheduleBooking.ScheduleId} not found.");
                }

                if ((newSchedule!.FinalPrice * booking.NumPeople) > booking.TotalPrice)
                {
                    booking.StatusId = 2;
                }
                else if ((newSchedule!.FinalPrice * booking.NumPeople) == booking.TotalPrice)
                {
                    booking.StatusId = 3;
                }

                booking.CountChangeLeft--;

                updateScheduleBooking.UpdateEntity(booking);
                await _context.SaveChangesAsync();

                var query = await _context.Bookings
                .Where(b => b.Id == booking.Id)
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
                .AsNoTracking().FirstOrDefaultAsync();

                var bookings = query!.Map();
                return Ok(new RestDTO<BookingDTO>()
                {
                    Data = bookings
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

                var query = await _context.Bookings
                .Where(b => b.Id == booking.Id)
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
                .AsNoTracking().FirstOrDefaultAsync();

                var bookings = query!.Map();
                return Ok(new RestDTO<BookingDTO>()
                {
                    Data = bookings
                });
            }
            catch (Exception ex)
            {
                return Problem($"Update fail {ex.Message}");
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

                .Include(s => s.Schedule)
                .ThenInclude(t => t!.Tour)
                .ThenInclude(tm => tm!.TourImages)
                .Where(b => b.Id == bookingId)
                .FirstOrDefaultAsync();

                if (booking == null)
                {
                    return Problem($"Id {bookingId} not found.");
                }

                if (booking.StatusId == 1)
                {
                    return Problem("Cannot delete schedule while booking is processing.");
                }

                var user = await _context.Users.FirstOrDefaultAsync(s => s.Id == booking.UserId);
                user!.Money = booking.TotalPrice;
                _context.Users.Update(user);

                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();

                return Ok(new RestDTO<Boolean>()
                {
                    Data = true
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

            UpdateScheduleBookingDTO updateBookingDTO = new UpdateScheduleBookingDTO()
            {
                Id = booking.Id,
                ScheduleId = changeBooking.ScheduleId,
            };

            return await updateScheduleBooking(updateBookingDTO);
        }
    }
}