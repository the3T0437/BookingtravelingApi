using BookingTravelApi.DTO.Tour;

namespace BookingTravelApi.DTO.schedule
{
    public class ScheduleDTOOfAccountant : ScheduleDTO
    {
        public int ProcessingBooking { get; set; }
        public int DepositBooking { get; set; }
        public int PaidBooking { get; set; }
    }
}