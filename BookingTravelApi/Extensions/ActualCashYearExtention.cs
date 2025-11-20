using BookingTravelApi.Domains;
using BookingTravelApi.DTO.actualYearDTO;
using BookingTravelApi.Helpers;

namespace BookingTravelApi.Extensions
{
    public static class ActualCashYearExtention
    {
        public static List<ActualCashYearDTO> MapYear(this List<Actualcashs> actualCash)
        {

            var timeNow = DateTimeHelper.GetVietNamTime();
            int currentYear = timeNow.Year;

            var moneyByYear = Enumerable.Range(0, 5)
            .ToDictionary(
                i => (currentYear - i).ToString(),
                i => 0
            );

            foreach (var item in actualCash)
            {
                string year = $"{item.CreatedAt.Year}";
                if (moneyByYear.ContainsKey(year))
                {
                    moneyByYear[year] += item.money;
                }
            }

            List<ActualCashYearDTO> lisItem = moneyByYear.Select(m => new ActualCashYearDTO
            {
                Year = m.Key,
                Value = m.Value
            }).ToList();

            return lisItem;
        }
    }
}