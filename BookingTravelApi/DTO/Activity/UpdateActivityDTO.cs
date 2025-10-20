using System.ComponentModel.DataAnnotations;

namespace BookingTravelApi.DTO.Activity
{
    public class UpdateActivityDTO
    {
        [Required]
        public int Id { get; set; }

        [MaxLength(255)]
        public String? Action { get; set; } = null!;
    }
}
