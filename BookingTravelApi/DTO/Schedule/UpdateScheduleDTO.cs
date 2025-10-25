using System.ComponentModel.DataAnnotations;
using BookingTravelApi.Domains;

namespace BookingTravelApi.DTO.schedule
{
    public class UpdateScheduleDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int TourId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public DateTime OpenDate { get; set; }

        [Required]
        [Range(0, 1000)]
        public int MaxSlot { get; set; }

        [Required]
        [Range(0, 100000000)]
        public int FinalPrice { get; set; }

        [Required]
        public TimeSpan GatheringTime { get; set; }

        [Required]
        [MaxLength(20)]
        public String Code { get; set; } = null!;

        [Required]
        [Range(0, 100)]
        public int Desposit { get; set; }

        public void UpdateEntity(Schedule schedule)
        {
            schedule.TourId = TourId;
            schedule.StartDate = StartDate;
            schedule.EndDate = EndDate;
            schedule.OpenDate = OpenDate;
            schedule.MaxSlot = MaxSlot;
            schedule.FinalPrice = FinalPrice;
            schedule.GatheringTime = GatheringTime;
            schedule.Code = Code;
            schedule.Desposit = Desposit;
        }
    }
}