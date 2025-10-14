namespace BookingTravelApi.Domains
{
    public class User
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public String Password { get; set; } = null!;
        public int Money { get; set; }
        public String BankNumber { get; set; } = null!;
        public String Bank { get; set; } = null!;
        public String Name { get; set; } = null!;
        public String Phone { get; set; } = null!;
        public String AvatarPath { get; set; } = null!;
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
