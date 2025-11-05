using BookingTravelApi.Domains;
using BookingTravelApi.DTO.favorite;

namespace BookingTravelApi.Extensions
{
    public static class FavoriteExtension
    {
        public static FavoriteDTO Map(this Favorite favorite)
        {
            return new FavoriteDTO()
            {
                UserId = favorite.UserId,
                TourId = favorite.TourId,

                Tour = favorite.Tour!.Map()
            };
        }
    }
}