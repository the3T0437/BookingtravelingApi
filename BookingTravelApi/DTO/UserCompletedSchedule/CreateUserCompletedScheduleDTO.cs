using System.ComponentModel.DataAnnotations;
using BookingTravelApi.Domains;

namespace BookingTravelApi.DTO.Activity
{
    public class CreateUserCompletedScheduleDTO
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int ScheduleId { get; set; }
        public UserCompletedSchedule Map()
        {
            return new UserCompletedSchedule()
            {
                UserId = UserId,
                ScheduleId = ScheduleId
            };
        }
    }
}
