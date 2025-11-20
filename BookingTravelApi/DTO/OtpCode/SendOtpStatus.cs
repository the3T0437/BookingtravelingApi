namespace BookingTravelApi.DTO.otpcode
{
    public class SendOtpStatus
    {
        public string Message { get; set; } = null!;
        public bool Status { get; set; }

        public SendOtpStatus(string error, bool status)
        {
            this.Message = error;
            this.Status = status;
        }
    }
}