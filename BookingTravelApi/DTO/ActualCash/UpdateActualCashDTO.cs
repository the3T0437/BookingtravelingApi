using System.ComponentModel.DataAnnotations;

namespace BookingTravelApi.DTO.updateActualCashDTO
{
    public class UpdateActualCashDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int money { get; set; }
        [Required]
        public int BookingId {get; set;}
    }
}