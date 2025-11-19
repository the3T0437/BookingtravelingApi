
using BookingTravelApi.Domains;
using BookingTravelApi.DTO.user;

namespace BookingTravelApi.Extensions
{
    public static class UserExtension
    {
        public static UserDTO Map(this User user)
        {
            return new UserDTO()
            {
                Id = user.Id,
                RoleId = user.RoleId,
                Money = user.Money,
                BankNumber = user.BankNumber,
                Bank = user.Bank,
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone,
                AvatarPath = user.AvatarPath,
                BankBranch = user.BankBranch,
                RefundStatus = user.RefundStatus
            };
        }
    }
}