using BookingTravelApi.Domains;
using BookingTravelApi.DTO.booking;
using BookingTravelApi.DTO.guide;
using BookingTravelApi.DTO.Helpful;

namespace BookingTravelApi.Extensions
{
    public static class HelpfulExtension
    {
        public static HelpfulDTO Map(this Helpful helpful)
        {
            return new HelpfulDTO()
            {
                ReviewId = helpful.ReviewId,
                UserId = helpful.UserId
            };
        } 
    }
}
