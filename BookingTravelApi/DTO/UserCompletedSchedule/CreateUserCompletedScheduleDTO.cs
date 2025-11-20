using System.ComponentModel.DataAnnotations;
using BookingTravelApi.Domains;

namespace BookingTravelApi.DTO.Activity
{
    public class CreateUserCompletedScheduleDTO
    {
        [Required]
<<<<<<< HEAD

        public int BookingId { get; set; }

        [Required]
        public int countPeople { get; set; }
=======
        public int BookingId { get; set; }

        [Required]
        public int countPeople { get; set; }

>>>>>>> main
        public UserCompletedSchedule Map()
        {
            return new UserCompletedSchedule()
            {
<<<<<<< HEAD
                BookingId =  this.BookingId,
                countPeople = this.countPeople
=======
                BookingId = BookingId,
                countPeople = countPeople,
                ScheduleId = 0,
                UserId = 0
>>>>>>> main
            };
        }
    }
}