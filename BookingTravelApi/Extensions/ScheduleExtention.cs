using BookingTravelApi.Domains;
using BookingTravelApi.DTO.schedule;

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
    }
}