using BookingTravelApi.Domains;
using BookingTravelApi.DTO.config;

namespace BookingTravelApi.Extensions
{
    public static class ConfigExtension
    {
        public static ConfigDTO Map(this Configs config)
        {
            return new ConfigDTO()
            {
                Id = config.Id,
                countChangeSchedule = config.countChangeSchedule,
                timeExpiredBookingHour = config.timeExpiredBookingHour,
                timeExpiredOtpSec = config.timeExpiredOtpSec,
            };
        }
    }
}