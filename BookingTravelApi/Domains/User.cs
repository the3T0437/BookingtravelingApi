using System.ComponentModel.DataAnnotations;

namespace BookingTravelApi.Domains
{
    public class User
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int RoleId { get; set; }

        [Required]
        [MaxLength(255)]
        public String Password { get; set; } = null!;
        [Required]
        public int Money { get; set; }

        [MaxLength(30)]
        public String BankNumber { get; set; } = null!;

        [MaxLength(255)]
        public String Bank { get; set; } = null!;

        [Required]
        [MaxLength(255)]
        public String Name { get; set; } = null!;

        [Required]
        [MaxLength(255)]
        public String Email { get; set; } = null!;

        [Required]
        [MaxLength(11)]
        public String Phone { get; set; } = null!;

        [Required]
        [MaxLength(255)]
        public String AvatarPath { get; set; } = null!;

        [Required]
        [MaxLength(255)]
        public String BankBranch { get; set; } = null!;

        public Staff? Staff { get; set; }
        public ICollection<Notification>? Notification { get; set; }
        public Role? Role { get; set; }
        public ICollection<Helpful>? Helpfuls { get; set; }
        public ICollection<Review>? Reviews { get; set; }
        public ICollection<UserCompletedSchedule>? UserCompletedSchedules { get; set; }
        public ICollection<Favorite>? Favorites { get; set; }
        public ICollection<Booking>? Bookings { get; set; }
    }
}
