using System.ComponentModel.DataAnnotations;
using BookingTravelApi.Domains;

namespace BookingTravelApi.DTO.place
{
    public class CreatePlaceDTO
    {
        [Required]
        public int LocationId { get; set; }

        [Required]
        [MaxLength(255)]
        public String Name { get; set; } = null!;

        public Place Map()
        {
            return new Place()
            {
                LocationId = LocationId,
                Name = Name
            };
        }
    }
}
