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
                UserId = booking.UserId,
                NumPeople = booking.NumPeople
            };
        }
    }
}