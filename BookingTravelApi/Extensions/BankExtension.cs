using BookingTravelApi.Domains;
using BookingTravelApi.DTO.bank;

namespace BookingTravelApi.Extensions
{
    public static class BankExtension
    {
        public static BankDTO Map(this Bank bank)
        {
            return new BankDTO()
            {
                id = bank.id,
                name = bank.name
            };
        }
    }
}