using BookingTravelApi.Domains;
using BookingTravelApi.DTO.booking;
using BookingTravelApi.DTO.notification;
using Microsoft.EntityFrameworkCore;
using PayOS;
using PayOS.Models.V2.PaymentRequests;

namespace BookingTravelApi.Services
{
    public class ChangeStatusBookingService
    {
        private readonly ApplicationDbContext _context;
        private readonly FirebaseNotificationService _notificationService;

        public ChangeStatusBookingService(ApplicationDbContext context, FirebaseNotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        public async Task<Booking?> ChangeStatusOfBooking(UpdateStatusBookingDTO updateStatusBooking)
        {
            var booking = await _context.Bookings
                .Include(i => i.User)
                .Include(i => i.Schedule)
                .FirstOrDefaultAsync(s => s.Id == updateStatusBooking.Id);
            if (booking == null)
            {
                return null;
            }

            var actualCash = await _context.Actualcashs.Where(i => i.BookingId == updateStatusBooking.Id).FirstOrDefaultAsync();
            var deposit = booking.Schedule!.Desposit * booking.Schedule.FinalPrice / 100 * booking.NumPeople;
            if (actualCash != null)
            {
                if (updateStatusBooking.StatusId == Status.Processing)
                {
                    actualCash.money -= deposit;
                }
                else
                {
                    actualCash.money += deposit;
                }
            }
            else
            {
                if (updateStatusBooking.StatusId == Status.Paid || updateStatusBooking.StatusId == Status.Deposit)
                {
                    actualCash = new Actualcashs()
                    {
                        BookingId = updateStatusBooking.Id,
                        CreatedAt = DateTime.UtcNow.AddHours(7),
                        money = deposit
                    };

                    await _context.Actualcashs.AddAsync(actualCash);

                    var type = ""; 
                    if (updateStatusBooking.StatusId == Status.Paid)
                    {
                        type = "Thanh toán toàn bộ";
                    }
                    else
                    {
                        type = "Đặt cọc";
                    }

                    var title = $"{type} thành công";
                    var body = $"Bạn đã {type} thành công cho lịch trình với mã {booking.Code}";
                    var notification = new CreateNotificationDTO()
                    {
                        UserId = booking.UserId,
                        Content = body
                    };
                    await _context.Notifications.AddAsync(notification.Map());
                    await _context.SaveChangesAsync();
                    await _notificationService.SendNotification(booking.User.Token, title, body);   
                }
            }

            updateStatusBooking.UpdateEntity(booking);
            await _context.SaveChangesAsync();

            return booking;
        }
    }
}