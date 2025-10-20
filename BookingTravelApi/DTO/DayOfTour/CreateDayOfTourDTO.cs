using System.ComponentModel.DataAnnotations;
using BookingTravelApi.DTO.DayActivity;

namespace BookingTravelApi.DTO.DayOfTour
{
    public class CreateDayOfTourDTO
    {
        [Required]
        public string Title { get; set; } = null!;
        [Required]
        public string Description { get; set; } = null!;
        [Required]
        public List<CreateDayActivityDTO> DayActivities { get; set; } = [];
    }

    public static class CreateDayOfTourDTOExtension
    {
        public static Domains.DayOfTour Map(this CreateDayOfTourDTO createDayOfTour)
        {
            var dayActivities = createDayOfTour.DayActivities.Select(i => i.Map()).ToList();
            return new Domains.DayOfTour()
            {
                Title = createDayOfTour.Title,
                Description = createDayOfTour.Description,
                DayActivities = dayActivities
            };
        }
    }
}