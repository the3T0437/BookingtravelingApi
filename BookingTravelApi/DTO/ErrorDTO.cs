namespace BookingTravelApi.DTO
{
    public class ErrorDTO
    {
        public ErrorDTO(String ErrorMessage)
        {
            this.ErrorMessage = ErrorMessage;
        }

        public String ErrorMessage { get; set; }
    }
}
