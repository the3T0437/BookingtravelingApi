using System.ComponentModel.DataAnnotations;
using BookingTravelApi.Domains;

namespace BookingTravelApi.DTO.Helpful
{
    public class ClickHelpfulRequest
    {

        [Required]
        public int UserId { get; set; }

        [Required]
        public int ReviewId { get; set; }

        public ClickHelpfulRequest(int UserId , int ReviewId)
        {
            this.UserId = UserId;
            this.ReviewId = ReviewId;
        }


        public Domains.Helpful Map()
        {
            return new Domains.Helpful()
            {
                ReviewId = ReviewId,
                UserId = UserId
            };
        }
    }
}
