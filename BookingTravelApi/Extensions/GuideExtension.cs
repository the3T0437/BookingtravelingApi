using BookingTravelApi.Domains;
using BookingTravelApi.DTO.booking;
using BookingTravelApi.DTO.guide;

namespace BookingTravelApi.Extensions
{
    public static class GuideExtension
    {
        public static GuideDTO Map(this Guide guide)
        {
            return new GuideDTO()
            {
                NameStaff = guide.Staff!.User!.Name,
                schedule = guide.Schedule!.Map()
            };
        } 
    }
}