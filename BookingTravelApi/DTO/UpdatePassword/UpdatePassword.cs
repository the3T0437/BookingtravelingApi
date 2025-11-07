namespace BookingTravelApi.DTO.updatePassword
{
    public class UpdatePassword
    {
        public string email { get; set; } = null!;
        public string oldPassword { get; set; } = null!;

        public string newPassword { get; set; } = null!;
    }
}