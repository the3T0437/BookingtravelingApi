using BookingTravelApi.Domains;
using BookingTravelApi.DTO.user;



namespace BookingTravelApi.DTO.staff
{
    public class CreateStaffDTO
    {
        public string Code { get; set; } = null!;
        public bool IsActive { get; set; }
        public string CCCD { get; set; } = null!;
        public string Address { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public DateTime StartWorkingDate { get; set; }
        public DateTime CCCDIssueDate { get; set; }
        public string CCCD_front_image { get; set; } = null!;
        public string CCCD_back_image { get; set; } = null!;
        public DateTime EndWorkingDate { get; set; }



        public CreateUserDTO? User { get; set; }

        public Staff Map()
        {
            return new Staff()
            {

                Code = Code,
                IsActive = IsActive,
                CCCD = CCCD,
                Address = Address,
                DateOfBirth = DateOfBirth,
                StartWorkingDate = StartWorkingDate,
                CCCDIssueDate = CCCDIssueDate,
                CCCD_front_path = "",
                CCCD_back_path = "",
                EndWorkingDate = EndWorkingDate,

                User = User!.Map(),
            };
        }
    }
}