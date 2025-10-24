using System.ComponentModel.DataAnnotations;
using BookingTravelApi.DTO.Activity;
using BookingTravelApi.DTO.LocationActivity;

namespace BookingTravelApi.DTO.DayActivity
{
    public class DayActivityDTO
    {
        [Required]
        public ActivityDTO? Activity { get; set; } = null!;

        [Required]
        public LocationActivityDTO? LocationActivity { get; set; } = null!;

        [Required]
        public TimeSpan Time { get; set; }
    }
}