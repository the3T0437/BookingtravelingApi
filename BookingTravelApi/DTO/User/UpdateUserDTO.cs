using System.ComponentModel.DataAnnotations;
using BookingTravelApi.Domains;

namespace BookingTravelApi.DTO.user
{
    public class UpdateUserDTO
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



        public void UpdateEntity(User user)
        {
            user.RoleId = RoleId;
            user.Password = Password;
            user.Money = Money;
            user.BankNumber = BankNumber;
            user.Bank = Bank;
            user.Name = Name;
            user.Email = Email;
            user.Phone = Phone;
            user.AvatarPath = AvatarPath;
            user.BankBranch = BankBranch;
        }
    }
}