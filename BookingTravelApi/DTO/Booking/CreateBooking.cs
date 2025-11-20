using System.ComponentModel.DataAnnotations;
using BookingTravelApi.Domains;
using BookingTravelApi.Helpers;

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
                CountChangeLeft = 3,
                CreatedAt = DateTimeHelper.GetVietNamTime(),
                ExpiredAt = DateTimeHelper.GetVietNamTime(),
                Qr = "",

            };
        }
    }
}