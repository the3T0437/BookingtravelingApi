using BookingTravelApi.DTO.role;
using BookingTravelApi.DTO.user;

namespace BookingTravelApi.DTO.staff
{
    public class StaffDTO
    {
        public int UserId { get; set; } 
        public string Code { get; set; } = null!;
        public bool IsActive { get; set; }
        public string CCCD { get; set; } = null!;
        public string Address { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public DateTime StartWorkingDate { get; set; }
        public DateTime CCCDIssueDate { get; set; }
        public string CCCD_front_path { get; set; } = null!;
        public string CCCD_back_path { get; set; } = null!;
        public DateTime EndWorkingDate { get; set; }



        public UserDTO User { get; set; } = null!;
        public RoleDTO Role { get; set; } = null!;
    }
}