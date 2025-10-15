using System.ComponentModel.DataAnnotations;

namespace BookingTravelApi.DTO.Activity
{
    public class ActivityDTO
    {
        public int Id { get; set; }

        public string Action { get; set; } = null!;
    }
}
