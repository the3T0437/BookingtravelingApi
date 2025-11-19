using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingTravelApi.Domains
{
    [Table("bookings")]
    public class Booking
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        public int ScheduleId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int StatusId { get; set; }

        [Required]
        public int NumPeople { get; set; }

        [Required]
        public String Code { get; set; } = null!;

        [Required]
        public String Email { get; set; } = null!;

        [Required]
        public String Phone { get; set; } = null!;

        [Required]
        public int TotalPrice { get; set; }

        [Required]
        public int CountChangeLeft { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public Status? Status { get; set; }
        public UserCompletedSchedule? UserCompletedSchedule {get; set;}
        public Schedule? Schedule { get; set; }
        public User? User { get; set; }
    }
}
