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
        public String Email { get; set; } = null!;

        [Required]
        public String Phone { get; set; } = null!;

        [Required]
        public int TotalPrice { get; set; }


        public Booking Map()
        {
            var random = new Random();
            return new Booking()
            {
                ScheduleId = ScheduleId,
                UserId = UserId,
                StatusId = Status.Processing,
                NumPeople = NumPeople,
                Code = "String",
                Email = Email,
                Phone = Phone,
                TotalPrice = TotalPrice,
<<<<<<< HEAD
                CountChangeLeft = 1,
                CreatedAt = DateTime.Now
=======
                CountChangeLeft = 3,
                CreatedAt = DateTime.Now,
                ExpiredAt = DateTime.Now,
                Qr = "",

>>>>>>> fe52fb30a5b25279854e0cd9cab57aae1ac9b685
            };
        }
    }
}