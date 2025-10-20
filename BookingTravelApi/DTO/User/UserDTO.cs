using BookingTravelApi.Domains;

namespace BookingTravelApi.DTO.user
{
    public class UserDTO
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int Money { get; set; }
        public String BankNumber { get; set; }
        public String Bank { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string AvatarPath { get; set; } = null!;
        public string BankBranch { get; set; } = null!;
    }
}