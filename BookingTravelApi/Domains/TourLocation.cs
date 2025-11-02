using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingTravelApi.Domains
{
    [Table("tourlocations")]
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
