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

    }
}
