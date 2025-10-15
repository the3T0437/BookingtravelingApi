using System.ComponentModel.DataAnnotations;

namespace BookingTravelApi.DTO.Activity
{
    public class CreateActivityDTO
    {
        [Required]
        [MaxLength(255)]
        public String Action { get; set; } = null!;

        public Domains.Activity Map()
        {
            return new Domains.Activity()
            {
                Action = Action
            };
        }
    }
}
