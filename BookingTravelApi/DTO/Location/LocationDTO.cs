using System.ComponentModel.DataAnnotations;

namespace BookingTravelApi.DTO.Location
{
    public class LocationDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;
    }
}
