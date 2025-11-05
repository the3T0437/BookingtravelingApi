namespace BookingTravelApi.DTO.ChangePassword
{
    public class ChangePassword
    {
        public string oldPassword { get; set; } = null!;

        public string newPassword { get; set; } = null!;
    }
}