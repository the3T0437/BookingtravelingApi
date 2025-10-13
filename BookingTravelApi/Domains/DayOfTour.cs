using System.ComponentModel.DataAnnotations;

namespace BookingTravelApi.Domains
{
    public class DayOfTour
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int TourId { get; set; }

        [Required]
        [Range(1, 50)]
        public int Day {  get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; } = null!; 

        [Required]
        [MaxLength(255)]
        public string Description { get; set; } = null!;
    }
}
