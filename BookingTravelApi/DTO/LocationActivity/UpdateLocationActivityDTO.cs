using System.ComponentModel.DataAnnotations;

namespace BookingTravelApi.DTO.LocationActivity
{
    public class UpdateLocationActivityDTO
    {
        [Required]
        public int Id { get; set; }
        public int? PlaceId { get; set; }

        [MaxLength(255)]
        public String? Name { get; set; } = null!;
    }
}
