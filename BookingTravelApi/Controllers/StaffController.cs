using System.Linq.Dynamic.Core;
using BookingTravelApi.Domains;
using BookingTravelApi.DTO.TourGuide;
using BookingTravelApi.DTO.staff;
using BookingTravelApi.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookingTravelApi.DTO;
using BookingTravelApi.Infrastructure;
using Microsoft.IdentityModel.Tokens;

namespace BookingTravelApi.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class StaffController : Controller
    {
        private ApplicationDbContext _context;
        private readonly ILogger<StaffController> _logger;
        public StaffController(ILogger<StaffController> logger, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> GetStaffs()
        {
            var query = _context.Staffs.Include(s => s.User).ThenInclude(u => u!.Role).AsNoTracking();

            var staffDTOs = await query.Select(i => i.Map()).ToArrayAsync();


            return Ok(new RestDTO<StaffDTO[]?>()
            {
                Data = staffDTOs
            });
        }


        [HttpGet()]
        [Route("tourguide/assignment/{idschedule}")]
        public async Task<IActionResult> GetTourGuide(int idschedule)
        {
            // Lấy thông tin schedule hiện tại
            var currentSchedule = await _context.Schedules
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == idschedule);

            if (currentSchedule == null)
            {
                return NotFound($"Schedule id {idschedule} not found");
            }

            // Lấy tất cả StaffIds đang bận 
            var busyStaffIds = await _context.Guides
                .Where(g => g.ScheduleId != idschedule &&
                        g.Schedule.StartDate <= currentSchedule.EndDate &&
                        g.Schedule.EndDate >= currentSchedule.StartDate)
                .Select(g => g.StaffId)
                .Distinct()
                .ToHashSetAsync();

            // Lấy các StaffIds đã được assign cho schedule này
            var assignedStaffIds = await _context.Guides
                .Where(g => g.ScheduleId == idschedule)
                .Select(g => g.StaffId)
                .ToHashSetAsync();

            // lọc ra các staff là tourguide và không nằm trong khoảng thời gian bận
            var availableTourGuides = await _context.Staffs
                .Where(s => s.User!.RoleId == 2 && !busyStaffIds.Contains(s.UserId))
                .AsNoTracking()
                .Select(s => new TourGuideDTO
                {
                    UserId = s.UserId,
                    Code = s.Code,
                    IsActive = s.IsActive,
                    CCCD = s.CCCD,
                    Address = s.Address,
                    DateOfBirth = s.DateOfBirth,
                    StartWorkingDate = s.StartWorkingDate,
                    CCCDIssueDate = s.CCCDIssueDate,
                    CCCD_front_path = s.CCCD_front_path,
                    CCCD_back_path = s.CCCD_back_path,
                    EndWorkingDate = s.EndWorkingDate,

                    ischecked = assignedStaffIds.Contains(s.UserId),
                    User = s.User!.Map(),
                })
                .ToArrayAsync();

            return Ok(new RestDTO<TourGuideDTO[]?>()
            {
                Data = availableTourGuides
            });
        }

        [HttpGet("{id}")]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> GetStaffById(int id)
        {
            var staff = await _context.Staffs
                .Include(s => s.User)
                .ThenInclude(u => u!.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.UserId == id);

            if (staff == null)
            {
                return NotFound($"id {id} not found");
            }

            return Ok(new RestDTO<StaffDTO?>()
            {
                Data = staff.Map()
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateStaff(CreateStaffDTO newStaffDTO)
        {
            try
            {
                var staff = newStaffDTO.Map();
                var listImages = new List<String>();
                listImages.Add(newStaffDTO.CCCD_front_image);
                listImages.Add(newStaffDTO.CCCD_back_image);

                var listPaths = await ImageInfrastructure.WriteImages(listImages);
                staff.CCCD_front_path = listPaths[0];
                staff.CCCD_back_path = listPaths[1];

                _context.Staffs.Add(staff);
                await _context.SaveChangesAsync();


                return await GetStaffById(staff.UserId);
            }
            catch (Exception ex)
            {
                return Problem("Error creating the staff: " + ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateStaff(UpdateStaffDTO updatedStaffDTO)
        {
            if (!updatedStaffDTO.IsRetainCCCDFront && updatedStaffDTO.CCCD_front_image.IsNullOrEmpty())
            {
                return BadRequest(new ErrorDTO("Chưa truyền ảnh trước CCCD"));
            }
            if (!updatedStaffDTO.IsRetainCCCDBack && updatedStaffDTO.CCCD_back_image.IsNullOrEmpty())
            {
                return BadRequest(new ErrorDTO("Chưa truyền ảnh trước CCCD"));
            }

            var tempImages = new List<String>();
            var oldImages = new List<String>();

            try
            {
                var staff = await _context.Staffs.Include(s => s.User).FirstOrDefaultAsync(s => s.UserId == updatedStaffDTO.UserId);

                if (staff == null)
                {
                    return NotFound($"id {updatedStaffDTO.UserId} not found");
                }

                updatedStaffDTO.UpdateEntity(staff);
                if (!updatedStaffDTO.IsRetainCCCDFront)
                {
                    var CCCDFront = await ImageInfrastructure.WriteImage(updatedStaffDTO.CCCD_front_image);
                    if (CCCDFront == null)
                    {
                        throw new Exception("Can't write CCCDFront image");
                    }
                    oldImages.Add(staff.CCCD_front_path);
                    tempImages.Add(CCCDFront);
                    staff.CCCD_front_path = CCCDFront;
                }
                if (!updatedStaffDTO.IsRetainCCCDBack)
                {
                    var CCCDBack = await ImageInfrastructure.WriteImage(updatedStaffDTO.CCCD_back_image);
                    if (CCCDBack == null)
                    {
                        throw new Exception("Can't write CCCDFront image");
                    }
                    oldImages.Add(staff.CCCD_back_path);
                    tempImages.Add(CCCDBack);
                    staff.CCCD_back_path = CCCDBack;
                }

                await _context.SaveChangesAsync();
                ImageInfrastructure.DeleteImages(oldImages);

                return await GetStaffById(staff.UserId);
            }
            catch (Exception ex)
            {
                ImageInfrastructure.DeleteImages(tempImages);
                return Problem("Error updating the staff: " + ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStaff(int id)
        {
            try
            {
                var staff = await _context.Staffs
                    .Include(s => s.User)
                    .ThenInclude(u => u!.Role)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(s => s.UserId == id);
                if (staff == null)
                {
                    return NotFound($"id {id} not found");
                }
                ImageInfrastructure.DeleteImage(staff.CCCD_front_path);
                ImageInfrastructure.DeleteImage(staff.CCCD_back_path);

                _context.Staffs.Remove(staff);
                await _context.SaveChangesAsync();

                return Ok(new RestDTO<StaffDTO>()
                {
                    Data = staff.Map()
                });
            }
            catch (Exception ex)
            {
                return Problem("Error deleting the staff: " + ex.Message);
            }
        }
    }
}