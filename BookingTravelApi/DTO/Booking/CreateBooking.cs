using System.ComponentModel.DataAnnotations;
using BookingTravelApi.Domains;

namespace BookingTravelApi.DTO.booking
{
    public class CreateBookingDTO
    {
        [Required]
        public int ScheduleId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int NumPeople { get; set; }

        [Required]
        public String Code { get; set; } = null!;

        [Required]
        public String Email { get; set; } = null!;

        [Required]
        public String Phone { get; set; } = null!;

        [Required]
        public int TotalPrice { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public Booking Map()
        {
            return new Booking()
            {
                ScheduleId = ScheduleId,
                UserId = UserId,
                StatusId = 1,
                NumPeople = NumPeople,
                Code = Code,
                Email = Email,
                Phone = Phone,
                TotalPrice = TotalPrice,
                CountChangeLeft = 3,
                CreatedAt = CreatedAt == default ? DateTime.Now : CreatedAt,
            };
        }
    }
}