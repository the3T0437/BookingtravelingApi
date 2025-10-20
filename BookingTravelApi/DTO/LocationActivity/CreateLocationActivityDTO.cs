using System.ComponentModel.DataAnnotations;
using BookingTravelApi.Domains;

namespace BookingTravelApi.DTO.LocationActivity
{
    public class CreateLocationActivityDTO
    {
        [Required]
        public int PlaceId { get; set; }

        [Required]
        [MaxLength(255)]
        public String Name { get; set; } = null!;

        [Required]
        public List<int> ActivityIds { get; set; } = [];

        public Domains.LocationActivity Map()
        {
            var activityAndLocations = ActivityIds.Select(i => new ActivityAndLocation() { ActivityId = i }).ToList();
            return new Domains.LocationActivity()
            {
                Name = Name,
                PlaceId = PlaceId,
                ActivityAndLocations = activityAndLocations
            };
        }
    }
}
