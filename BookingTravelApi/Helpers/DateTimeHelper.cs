namespace BookingTravelApi.Helpers
{
    public static class DateTimeHelper
    {
        public static DateTime GetVietNamTime()
        {
            TimeZoneInfo vietnamZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vietnamZone);
        }

    }
}