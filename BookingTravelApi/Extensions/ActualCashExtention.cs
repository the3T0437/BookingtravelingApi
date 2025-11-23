using BookingTravelApi.Domains;
using BookingTravelApi.DTO.actualCashDTO;

namespace BookingTravelApi.Extensions
{
    public static class ActualCashExtention
    {
        public static ActualCashDTO Map(this Actualcashs actualcash)
        {
            return new ActualCashDTO()
            {
              Id = actualcash.Id,
              money = actualcash.money,
              BookingId = actualcash.BookingId,
              CreatedAt = actualcash.CreatedAt  
            };
        }
    }
}