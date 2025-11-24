using BookingTravelApi.Domains;
using BookingTravelApi.DTO.booking;
using Microsoft.EntityFrameworkCore;
using PayOS;
using PayOS.Models.V2.PaymentRequests;

namespace BookingTravelApi.Services
{
    public class ChangeStatusBookingService
    {
        private readonly ApplicationDbContext _context;

        public ChangeStatusBookingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Booking?> ChangeStatusOfBooking(UpdateStatusBookingDTO updateStatusBooking)
        {
            var booking = await _context.Bookings.Include(i => i.Schedule).FirstOrDefaultAsync(s => s.Id == updateStatusBooking.Id);
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
                }
            }

            updateStatusBooking.UpdateEntity(booking);
            await _context.SaveChangesAsync();

            return booking;
        }
    }
}