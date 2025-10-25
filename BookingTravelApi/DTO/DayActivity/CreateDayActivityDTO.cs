using System.ComponentModel.DataAnnotations;
using BookingTravelApi.DTO.Activity;
using BookingTravelApi.DTO.LocationActivity;

namespace BookingTravelApi.DTO.DayActivity
{
    public class CreateDayActivityDTO
    {
        [Required]
        public int ActivityId { get; set; }

        [Required]
        public int LocationActivityId { get; set; }

        [Required]
        public TimeSpan Time { get; set; }
    }

    public static class CreateDayActivityExtension
    {
        public static Domains.DayActivity Map(this CreateDayActivityDTO createDayActivityDTO)
        {
            return new Domains.DayActivity()
            {
                ActivityId = createDayActivityDTO.ActivityId,
                LocationActivityId = createDayActivityDTO.LocationActivityId,
                Time = createDayActivityDTO.Time
            };
        }
    }
}