using System.ComponentModel.DataAnnotations;
using BookingTravelApi.Domains;

namespace BookingTravelApi.DTO.booking
{
    public class ChangeBookingDTO
    {
        [Required]
        public int ScheduleId { get; set; }

        [Required]
        public int BookingId { get; set; }
    }
}