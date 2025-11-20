namespace BookingTravelApi.DTO.otpcode
{
    public class SendOtpStatus
    {
        public string error { get; set; } = null!;
        public bool status { get; set; }

        public SendOtpStatus(string error, bool status)
        {
            this.error = error;
            this.status = status;
        }
    }
}