using System.ComponentModel.DataAnnotations;
using BookingTravelApi.Domains;
using BookingTravelApi.Helpers;

namespace BookingTravelApi.DTO.createActualCashDTO
{
    public class CreateActualCashDTO
    {
        [Required]
        public int money { get; set; }
        [Required]
        public int BookingId { get; set; }

        public Actualcashs Map()
        {
            return new Actualcashs
            {
                money = money,
                CreatedAt = DateTimeHelper.GetVietNamTime(),
                BookingId = BookingId
            };
        }
    }
}