using System.ComponentModel.DataAnnotations;

namespace BookingTravelApi.DTO.Activity
{
    public class UpdateActivityDTO
    {
        [MaxLength(255)]
        public String Action { get; set; } = null!;
    }
}
