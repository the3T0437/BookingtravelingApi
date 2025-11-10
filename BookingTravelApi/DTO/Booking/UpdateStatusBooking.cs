using System.ComponentModel.DataAnnotations;
using BookingTravelApi.Domains;

namespace BookingTravelApi.DTO.booking
{
    public class UpdateStatusBookingDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int StatusId { get; set; }

        public void UpdateEntity(Booking booking)
        {
            booking.StatusId = StatusId;
        }
    }
}