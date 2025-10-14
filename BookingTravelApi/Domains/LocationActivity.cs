using System.ComponentModel.DataAnnotations;

namespace BookingTravelApi.Domains
{
    public class LocationActivity
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        public int PlaceId { get; set; }

        [Required]
        [MaxLength(255)]
        public String Name { get; set; } = null!;

        public ICollection<ActivityAndLocation>? ActivityAndLocations { get; set; }
        public ICollection<DayActivity>? DayActivities { get; set; }
        public Place? Place { get; set; }
    }
}
