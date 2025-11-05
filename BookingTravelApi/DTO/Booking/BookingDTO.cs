using BookingTravelApi.Domains;
using BookingTravelApi.DTO.schedule;
using BookingTravelApi.DTO.status;
using BookingTravelApi.DTO.user;

namespace BookingTravelApi.DTO.booking
{
    public class BookingDTO
    {
        public int Id { get; set; }
        public int NumPeople { get; set; }
        public String Code { get; set; } = null!;
        public String Email { get; set; } = null!;
        public String Phone { get; set; } = null!;
        public int TotalPrice { get; set; }
        public int CountChangeLeft { get; set; }
        public DateTime CreatedAt { get; set; }

        public StatusDTO? Status { get; set; }
        public ScheduleDTO Schedule { get; set; } = null!;
        public UserDTO? User { get; set; }
    }
}
