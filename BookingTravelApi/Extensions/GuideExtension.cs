using BookingTravelApi.Domains;
using BookingTravelApi.DTO.booking;
using BookingTravelApi.DTO.guide;

namespace BookingTravelApi.Extensions
{
    public static class GuideExtension
    {
        public static GuideDTO Map(this Guide guide)
        {
            // Lấy User ID
            var completedUserIds = guide.Schedule?.UserCompletedSchedules?.Where(ucs => ucs.User != null).Select(ucs => ucs.User!.Id).ToHashSet();
            // Lọc các Booking
            var filteredBookings = guide.Schedule?.Bookings?.Where(b => completedUserIds != null && completedUserIds.Contains(b.UserId)).ToList();

            var relevantBookingDTOs = filteredBookings?.Select(b => b.Map()).ToList() ?? new List<BookingDTO>();

            return new GuideDTO()
            {
                //new
                NameStaff = guide.Staff!.User!.Name,

                Schedule = guide.Schedule!.Map(),
                Booking = relevantBookingDTOs
            };
        } 
    }
}
