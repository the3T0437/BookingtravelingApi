using System.ComponentModel.DataAnnotations;

namespace BookingTravelApi.DTO.place
{
    public class UpdatePlaceDTO
    {
        [Required]
        public int Id { get; set; }
        public int? LocationId { get; set; }

        [MaxLength(255)]
        public String? Name { get; set; } = null!;
    }
}
