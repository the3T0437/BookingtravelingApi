using BookingTravelApi.Domains;
using BookingTravelApi.DTO.Activity;
using BookingTravelApi.DTO.DayOfTour;
using BookingTravelApi.DTO.guide;
using BookingTravelApi.DTO.schedule;
using BookingTravelApi.DTO.staff;
using BookingTravelApi.DTO.Tour;
using BookingTravelApi.DTO.user;


namespace BookingTravelApi.DTO.review
{
    public class ReviewDTO
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public String Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        
        public UserDTO? User { get; set; }
        public List<GuideDTO>? Guides { get; set; }
        
    }
}