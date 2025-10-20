using BookingTravelApi.Domains;

namespace BookingTravelApi.DTO.schedule
{
    public class ScheduleDTO
    {
        public int Id { get; set; }
        public int TourId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime OpenDate { get; set; }
        public int MaxSlot { get; set; }
        public int FinalPrice { get; set; }
        public DateTime GatheringTime { get; set; }
        public String Code { get; set; } = null!;
        public int Desposit { get; set; }


        public Domains.Tour tour { get; set; } = null!;
    }
}