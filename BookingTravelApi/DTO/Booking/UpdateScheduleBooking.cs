using System.ComponentModel.DataAnnotations;
using BookingTravelApi.Domains;

namespace BookingTravelApi.DTO.booking
{
    public class UpdateScheduleBookingDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int ScheduleId { get; set; }


        public void UpdateEntity(Booking booking)
        {
            booking.ScheduleId = ScheduleId;
        }
    }
}