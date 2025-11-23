using System.ComponentModel.DataAnnotations;
using BookingTravelApi.Domains;

namespace BookingTravelApi.DTO.user
{
    public class CreateUserDTO
    {
        [Required]
        public int RoleId { get; set; }

        [MaxLength(255)]
        public String? Password { get; set; }

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

        [MaxLength(255)]
        public String AvatarPath { get; set; } = null!;

        [MaxLength(255)]
        public String BankBranch { get; set; } = null!;

        [MaxLength(255)]
        public string? Token { get; set; }

        public CreateUserDTO(
             int roleId,
             string password,
             string name,
             string email,
             string? phone = null,
             int money = 0,
             string? bankNumber = null,
             string? bank = null,
             string? avatarPath = null,
             string? bankBranch = null,
             string? Token = null)
        {
            RoleId = roleId;
            Password = password;
            Name = name;
            Email = email;
            Phone = phone;
            Money = money;
            BankNumber = bankNumber ?? "string";
            Bank = bank ?? "string";
            AvatarPath = avatarPath ?? "string";
            BankBranch = bankBranch ?? "string";
            this.Token = Token;
        }

        public User Map()
        {
            return new User()
            {
                RoleId = RoleId,
                Password = Password,
                Money = Money,
                BankNumber = BankNumber,
                Bank = Bank,
                Name = Name,
                Email = Email,
                Phone = Phone,
                AvatarPath = AvatarPath,
                BankBranch = BankBranch,
                RefundStatus = false,
                Token = Token
            };
        }
    }
}