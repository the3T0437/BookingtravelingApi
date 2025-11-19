using BookingTravelApi.Domains;
using BookingTravelApi.DTO.schedule;
using BookingTravelApi.DTO.ScheduleAssignmentDTO;

namespace BookingTravelApi.Extensions
{
    public static class ScheduleExtention
    {
        public static ScheduleDTO Map(this Schedule schedule)
        {
            return new ScheduleDTO()
            {
                Id = schedule.Id,
                TourId = schedule.TourId,
                StartDate = schedule.StartDate,
                EndDate = schedule.EndDate,
                OpenDate = schedule.OpenDate,
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
            return new ScheduleDTOOfAccountant()
            {
                Id = schedule.Id,
                TourId = schedule.TourId,
                StartDate = schedule.StartDate,
                EndDate = schedule.EndDate,
                OpenDate = schedule.OpenDate,
                MaxSlot = schedule.MaxSlot,
                FinalPrice = schedule.FinalPrice,
                GatheringTime = schedule.GatheringTime,
                Code = schedule.Code,
                Desposit = schedule.Desposit,

                tour = schedule.Tour!.Map(),

                ProcessingBooking = schedule.Bookings!.Where(s => s.StatusId == Status.Paid).Count(),
                DepositBooking = schedule.Bookings!.Where(s => s.StatusId == Status.Paid).Count(),
                PaidBooking = schedule.Bookings!.Where(s => s.StatusId == Status.Paid).Count(),
            };
        }
    }
}