using System.ComponentModel.DataAnnotations;
using BookingTravelApi.Domains;
using BookingTravelApi.DTO.DayOfTour;
using BookingTravelApi.DTO.Location;
using BookingTravelApi.DTO.place;

namespace BookingTravelApi.DTO.Tour
{
    public class TourDTO
    {
        public int Id { get; set; }
        public int Day { get; set; }
        public String Title { get; set; } = null!;
        public int Price { get; set; }
        public int PercentDeposit { get; set; }
        public String Description { get; set; } = null!;

        public List<DayOfTourDTO> DayOfTours { get; set; } = null!;
        public List<String> TourImages { get; set; } = null!;
        public List<LocationDTO> Locations { get; set; } = null!;
        public List<PlaceDTO> Places { get; set; } = null!;
        public int TotalReviews { get; set; }
        public int TotalStars { get; set; }

    }
}