using BookingTravelApi.Domains;
using BookingTravelApi.DTO.booking;
using BookingTravelApi.DTO.schedule;
using BookingTravelApi.DTO.ScheduleAssignmentDTO;

namespace BookingTravelApi.Extensions
{
    public static class ScheduleExtention
    {
        public static ScheduleDTO Map(this Schedule schedule)
        {
            var paidBooking = schedule.Bookings?.Where(i => i.StatusId != Status.Processing).Select(i => i.NumPeople).Sum() ?? 0;
            var processingBooking = schedule.Bookings?.Where(i => i.StatusId == Status.Processing && i.ExpiredAt > DateTime.UtcNow.AddHours(7)).Select(i => i.NumPeople).Sum() ?? 0;

            return new ScheduleDTO()
            {
                Id = schedule.Id,
                TourId = schedule.TourId,
                StartDate = schedule.StartDate,
                EndDate = schedule.EndDate,
                OpenDate = schedule.OpenDate,
                BookedSlot = paidBooking + processingBooking,
                MaxSlot = schedule.MaxSlot,
                FinalPrice = schedule.FinalPrice,
                GatheringTime = schedule.GatheringTime,
                Code = schedule.Code,
                Desposit = schedule.Desposit,
                tour = schedule.Tour!.Map(),
            };
        }

        public static ScheduleDTOOfAccountant MapToScheduleOfAccountant(this Schedule schedule)
        {
            var paidBooking = schedule.Bookings?.Where(i => i.StatusId != Status.Processing).Select(i => i.NumPeople).Sum() ?? 0;
            var processingBooking = schedule.Bookings?.Where(i => i.StatusId == Status.Processing && i.ExpiredAt > DateTime.UtcNow.AddHours(7)).Select(i => i.NumPeople).Sum() ?? 0;

            return new ScheduleDTOOfAccountant()
            {
                Id = schedule.Id,
                TourId = schedule.TourId,
                StartDate = schedule.StartDate,
                EndDate = schedule.EndDate,
                OpenDate = schedule.OpenDate,
                MaxSlot = schedule.MaxSlot,
                BookedSlot = paidBooking + processingBooking,
                FinalPrice = schedule.FinalPrice,
                GatheringTime = schedule.GatheringTime,
                Code = schedule.Code,
                Desposit = schedule.Desposit,

                tour = schedule.Tour!.Map(),

                ProcessingBooking = schedule.Bookings!.Where(s => s.StatusId == Status.Processing).Count(),
                DepositBooking = schedule.Bookings!.Where(s => s.StatusId == Status.Deposit).Count(),
                PaidBooking = schedule.Bookings!.Where(s => s.StatusId == Status.Paid).Count(),
            };
        }

        public static ScheduleDTOOfAdmin MapToScheduleOfAdmin(this Schedule schedule)
        {
            var paidBooking = schedule.Bookings?.Where(i => i.StatusId != Status.Processing).Select(i => i.NumPeople).Sum() ?? 0;
            var processingBooking = schedule.Bookings?.Where(i => i.StatusId == Status.Processing && i.ExpiredAt > DateTime.UtcNow.AddHours(7)).Select(i => i.NumPeople).Sum() ?? 0;

            var totalStars = schedule.Reviews!.Select(i => i.Rating).Sum();
            var totalReviews = schedule.Reviews!.Count();

            return new ScheduleDTOOfAdmin()
            {
                Id = schedule.Id,
                TourId = schedule.TourId,
                StartDate = schedule.StartDate,
                EndDate = schedule.EndDate,
                OpenDate = schedule.OpenDate,
                MaxSlot = schedule.MaxSlot,
                BookedSlot = paidBooking + processingBooking,
                FinalPrice = schedule.FinalPrice,
                GatheringTime = schedule.GatheringTime,
                Code = schedule.Code,
                Desposit = schedule.Desposit,

                tour = schedule.Tour!.Map(),
                TotalReviews = totalReviews,
                TotalStars = totalStars
            };
        }
    }
}