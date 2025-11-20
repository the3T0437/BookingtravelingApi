using System.ComponentModel.DataAnnotations;
using BookingTravelApi.Domains;

namespace BookingTravelApi.DTO.Activity
{
    public class CreateUserCompletedScheduleDTO
    {
        [Required]

        public int BookingId { get; set; }

        [Required]
        public int countPeople { get; set; }
        public UserCompletedSchedule Map()
        {
            return new UserCompletedSchedule()
            {
                BookingId =  this.BookingId,
                countPeople = this.countPeople
            };
        }
    }
}