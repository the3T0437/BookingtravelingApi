using System.ComponentModel.DataAnnotations;
using BookingTravelApi.Domains;

namespace BookingTravelApi.DTO.place
{
    public class CreateTourDTO
    {
        [Required]
        public int TotalRevenue { get; set; }

        [Required]
        public int PercentDeposit { get; set; }

        [Required]
        public int Day { get; set; }

        [Required]
        [MaxLength(255)]
        public String Title { get; set; } = null!;

        [Required]
        [Range(0, 100000000)]
        public int Price { get; set; }

        [Required]
        [MaxLength(255)]
        public String Description { get; set; } = null!;
        public Tour Map()
        {
            return new Tour()
            {
                TotalRevenue = TotalRevenue,
                PercentDeposit = PercentDeposit,
                Day = Day,
                Title = Title,
                Price = Price,
                Description = Description
            };
        }
    }
}