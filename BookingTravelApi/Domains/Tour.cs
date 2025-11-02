using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingTravelApi.Domains
{
    [Table("tours")]
    public class Tour
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int TotalRevenue { get; set; }

        [Required]
        public int PercentDeposit { get; set; }

        [Required]
        public int Day { get; set; }

        [Required]
        [MaxLength(255)]
        public String Title { get; set; } = null!;

        [Required]
        [Range(0, 100000000)]
        public int Price { get; set; }

        [Required]
        [MaxLength(255)]
        public String Description { get; set; } = null!;

        [Required]
        public int TotalReviews { get; set; } = 0;

        [Required]
        public int TotalStars { get; set; } = 0;

        public ICollection<TourImage>? TourImages { get; set; }
        public ICollection<DayOfTour>? DayOfTours { get; set; }
        public ICollection<TourLocation>? TourLocations { get; set; }

        public ICollection<Schedule>? Schedules { get; set; }
        public ICollection<Favorite>? Favorites { get; set; }
    }
}
