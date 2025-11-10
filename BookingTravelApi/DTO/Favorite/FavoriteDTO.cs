using BookingTravelApi.DTO.Tour;
using BookingTravelApi.DTO.user;

namespace BookingTravelApi.DTO.favorite
{
    public class FavoriteDTO
    {
        public int UserId { get; set; }
        public int TourId { get; set; }

        public TourDTO? Tour { get; set; }
    }
}