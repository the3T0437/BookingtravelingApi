using BookingTravelApi.Domains;
using BookingTravelApi.DTO.booking;

namespace BookingTravelApi.Extensions
{
    public static class BookingExtension
    {
        public static BookingDTO Map(this Booking booking)
        {
            return new BookingDTO()
            {
                Id = booking.Id,
                NumPeople = booking.NumPeople,
                Code = booking.Code,
                Email = booking.Email,
                Phone = booking.Phone,
                TotalPrice = booking.TotalPrice,
                CountChangeLeft = booking.CountChangeLeft,
                CreatedAt = booking.CreatedAt,
                PayType = false,
                ExpiredAt = booking.ExpiredAt,               
                Qr = booking.Qr,
                Status = booking.Status!.Map(),
                Schedule = booking.Schedule!.Map(),
                User = booking.User!.Map()
            };
        }
    }
}