using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingTravelApi.Domains
{
    [Table("staffs")]
    public class Staff
    {
        [Key]
        [Required]
        public int UserId { get; set; }

        [Required]
        [MaxLength(20)]
        public String Code { get; set; } = null!;

        [Required]
        public bool IsActive { get; set; }

        [Required]
        [MaxLength(50)]
        public String CCCD { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public String Address { get; set; } = null!;

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public DateTime StartWorkingDate { get; set; }

        [Required]
        public DateTime CCCDIssueDate { get; set; }

        [Required]
        [MaxLength(255)]
        public String CCCD_front_path { get; set; } = null!;

        [Required]
        [MaxLength(255)]
        public String CCCD_back_path { get; set; } = null!;

        [Required]
        public DateTime EndWorkingDate { get; set; }

        public User? User { get; set; }
        public ICollection<Guide>? Guides { get; set; }
    }
}
