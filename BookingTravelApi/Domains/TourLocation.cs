using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.ComponentModel.DataAnnotations;

namespace BookingTravelApi.Domains
{
    public class TourLocation
    {
        [Required]
        [Key]
        public int TourId { get; set; }

        [Required]
        [Key]
        public int LocationId { get; set; }

        public Tour? Tour { get; set; }
        public Location? Location { get; set; }
    }
}
