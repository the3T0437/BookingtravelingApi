using System.ComponentModel.DataAnnotations;

namespace BookingTravelApi.DTO
{
    public class UpdateUserCompletedScheduleDTO
    {
        [Required]
        public int BookingId {get; set;}
        public int countPeople {get; set;}
        
    }
}