using System.ComponentModel.DataAnnotations;
using BookingTravelApi.Domains;

namespace BookingTravelApi.DTO.LocationActivity
{
    public class CreateLocationActivityDTO
    {
        [Required]
        public int PlaceId { get; set; }

        [Required]
        [MaxLength(255)]
        public String Name { get; set; } = null!;

        public Domains.LocationActivity Map()
        {
            return new Domains.LocationActivity()
            {
                Name = Name,
                PlaceId = PlaceId
            };
        }
    }
}
