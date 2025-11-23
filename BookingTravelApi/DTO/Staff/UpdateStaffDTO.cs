using System.ComponentModel.DataAnnotations;
using BookingTravelApi.Domains;
using BookingTravelApi.DTO.user;

namespace BookingTravelApi.DTO.staff
{
    public class UpdateStaffDTO
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
        public String CCCD_front_image { get; set; } = null!;

        [Required]
        public String CCCD_back_image { get; set; } = null!;

        public bool IsRetainCCCDFront { get; set; } = true;

        public bool IsRetainCCCDBack { get; set; } = true;

        [Required]
        public DateTime EndWorkingDate { get; set; }

        public UpdateUserDTO? User { get; set; }


        public void UpdateEntity(Staff staff)
        {
            staff.Code = Code;
            staff.IsActive = IsActive;
            staff.CCCD = CCCD;
            staff.Address = Address;
            staff.DateOfBirth = DateOfBirth;
            staff.StartWorkingDate = StartWorkingDate;
            staff.CCCDIssueDate = CCCDIssueDate;
            staff.EndWorkingDate = EndWorkingDate;

            if (staff.User != null)
            {
                User?.UpdateEntity(staff.User);
            }
        }
    }
}