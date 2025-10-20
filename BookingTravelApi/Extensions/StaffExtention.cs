using BookingTravelApi.Domains;
using BookingTravelApi.DTO.staff;

namespace BookingTravelApi.Extensions
{
    public static class StaffExtention
    {
        public static StaffDTO Map(this Staff staff)
        {
            return new StaffDTO()
            {
                UserId = staff.UserId,
                Code = staff.Code,
                IsActive = staff.IsActive,
                CCCD = staff.CCCD,
                Address = staff.Address,
                DateOfBirth = staff.DateOfBirth,
                StartWorkingDate = staff.StartWorkingDate,
                CCCDIssueDate = staff.CCCDIssueDate,
                CCCD_front_path = staff.CCCD_front_path,
                CCCD_back_path = staff.CCCD_back_path,
                EndWorkingDate = staff.EndWorkingDate,



                User = staff.User!.Map(),
                Role = staff.User!.Role!.Map()
            };
        }
    }
}