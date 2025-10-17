using BookingTravelApi.Domains;
using BookingTravelApi.DTO.role;

namespace BookingTravelApi.Extensions
{
    public static class RoleExtension
    {
        public static RoleDTO Map(this Role role)
        {
            return new RoleDTO()
            {
                Id = role.Id,
                Title = role.Title
            };
        }
    }
}