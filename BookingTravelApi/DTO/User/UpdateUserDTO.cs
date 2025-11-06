using System.ComponentModel.DataAnnotations;
using BookingTravelApi.Domains;

namespace BookingTravelApi.DTO.user
{
    public class UpdateUserDTO
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public int Money { get; set; }

        [MaxLength(30)]
        public String BankNumber { get; set; } = null!;

        [MaxLength(255)]
        public String Bank { get; set; } = null!;

       
        [MaxLength(255)]
        public String Name { get; set; } = null!;

       
        [MaxLength(255)]
        public String Email { get; set; } = null!;

       
        [MaxLength(11)]
        public String Phone { get; set; } = null!;

        
        [MaxLength(255)]
        public String AvatarPath { get; set; } = null!;

        [MaxLength(255)]
        public String BankBranch { get; set; } = null!;



        public void UpdateEntity(User user)
        {
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