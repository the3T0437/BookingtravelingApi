using System.ComponentModel.DataAnnotations;

namespace BookingTravelApi.DTO.config
{
    public class ConfigDTO
    {
        public int Id { get; set; }
        public String Name { get; set; } = null!;
        public int Value { get; set; }

    }
}