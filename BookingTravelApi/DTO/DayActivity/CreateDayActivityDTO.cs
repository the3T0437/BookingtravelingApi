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
        public DateTime Time { get; set; }
    }

    public static class CreateDayActivityExtension
    {
        public static Domains.DayActivity Map(this CreateDayActivityDTO createDayActivityDTO, int dayOfTourId)
        {
            return new Domains.DayActivity()
            {
                DayOfTourId = dayOfTourId,
                ActivityId = createDayActivityDTO.ActivityId,
                LocationActivityId = createDayActivityDTO.LocationActivityId,
                Time = createDayActivityDTO.Time
            };
        }
    }
}