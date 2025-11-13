using System.ComponentModel.DataAnnotations;
using BookingTravelApi.Domains;

namespace BookingTravelApi.DTO.user
{
    public class CreateUserDTO
    {
        [Required]
        public int RoleId { get; set; }

        [Required]
        [MaxLength(255)]
        public String Password { get; set; } = null!;

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
                RefundStatus = false
            };
        }
    }
}