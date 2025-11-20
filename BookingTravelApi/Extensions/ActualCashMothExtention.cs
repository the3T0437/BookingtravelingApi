using BookingTravelApi.Domains;
using BookingTravelApi.DTO.actualCashDTO;

namespace BookingTravelApi.Extensions
{
    public static class ActualCashMothExtention
    {
        public static ActualCashMonthDTO MapMonth(this List<Actualcashs> actualCash)
        {
            int[] moneys = new int[12];
            actualCash.ForEach((it) =>
            {
                moneys[it.CreatedAt.Month - 1] += it.money;
            });

            return new ActualCashMonthDTO
            {
                January = moneys[0],
                February = moneys[1],
                March = moneys[2],
                April = moneys[3],
                May = moneys[4],
                June = moneys[5],
                July = moneys[6],
                August = moneys[7],
                September = moneys[8],
                October = moneys[9],
                November = moneys[10],
                December = moneys[11],
            };
        }
    }
}