using BookingTravelApi.DTO.Tour;

namespace BookingTravelApi.DTO.schedule
{
    public class ScheduleDTOOfAccountant
    {
        public int Id { get; set; }
        public int TourId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime OpenDate { get; set; }
        public int MaxSlot { get; set; }
        public int FinalPrice { get; set; }
        public TimeSpan GatheringTime { get; set; }
        public String Code { get; set; } = null!;
        public int Desposit { get; set; }
        public TourDTO tour { get; set; } = null!;
        public int ProcessingBooking { get; set; }
        public int DepositBooking { get; set; }
        public int PaidBooking { get; set; }
    }
}