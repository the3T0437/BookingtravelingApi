using System.ComponentModel.DataAnnotations;
using BookingTravelApi.Domains;

namespace BookingTravelApi.DTO.Activity
{
    public class CreateGuideDTO
    {
        [Required]
        public int StaffId { get; set; }

        [Required]
        public int ScheduleId { get; set; }

        public Guide Map()
        {
            return new Guide()
            {
                StaffId = StaffId,
                ScheduleId = ScheduleId
            };
        }
    }
}
