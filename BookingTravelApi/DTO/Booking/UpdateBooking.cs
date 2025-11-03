using System.ComponentModel.DataAnnotations;
using BookingTravelApi.Domains;

namespace BookingTravelApi.DTO.booking
{
    public class UpdateBookingDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int ScheduleId { get; set; }

        [Required]
        public int StatusId { get; set; }

        [Required]
        public int CountChangeLeft { get; set; }

        public void UpdateEntity(Booking booking)
        {
            booking.ScheduleId = ScheduleId;
            booking.StatusId = StatusId;
            booking.CountChangeLeft = CountChangeLeft;
        }
    }
}