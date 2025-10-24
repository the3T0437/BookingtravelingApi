using System.ComponentModel.DataAnnotations;

namespace BookingTravelApi.Domains
{
    public class Schedule
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        public int TourId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public DateTime OpenDate { get; set; }

        [Required]
        [Range(0, 1000)]
        public int MaxSlot { get; set; }

        [Required]
        [Range(0, 100000000)]
        public int FinalPrice { get; set; }

        [Required]
        public TimeSpan GatheringTime { get; set; }

        [Required]
        [MaxLength(20)]
        public String Code { get; set; } = null!;

        [Required]
        [Range(0, 100)]
        public int Desposit { get; set; }

        public Tour? Tour { get; set; }
        public ICollection<Booking>? Bookings { get; set; }
        public ICollection<UserCompletedSchedule>? UserCompletedSchedules { get; set; }
        public ICollection<Review>? Reviews { get; set; }
        public ICollection<Guide>? Guides { get; set; }
    }
}
