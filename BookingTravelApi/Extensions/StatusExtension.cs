using BookingTravelApi.Domains;
using BookingTravelApi.DTO.status;

namespace BookingTravelApi.Extensions
{
    public static class StatusExtension
    {
        public static StatusDTO Map(this Status status)
        {
            return new StatusDTO()
            {
                Id = status.Id,
                Name = status.Name
            };
        }
    }
}
