using System.ComponentModel.DataAnnotations;
using BookingTravelApi.Domains;

namespace BookingTravelApi.DTO.booking
{
    public class CreateFavoriteDTO
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int TourId { get; set; }

        public Favorite Map()
        {
            return new Favorite()
            {
                UserId = UserId,
                TourId = TourId
            };
        }
    }
}