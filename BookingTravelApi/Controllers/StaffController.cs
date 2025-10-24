using System.Linq.Dynamic.Core;
using BookingTravelApi.Domains;
using BookingTravelApi.DTO;
using BookingTravelApi.DTO.staff;
using BookingTravelApi.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        [Route("tourguide/assignment")]
        public async Task<IActionResult> GetTourGuide()
        {
            var timeNow = DateTime.Now;

            // lọc ra các id của schedule mà đang tiến hành 
            var activeScheduleIds = await _context.Schedules.Where(s => s.StartDate <= timeNow && s.EndDate >= timeNow)
            .Select(s => s.Id).ToArrayAsync();

            // lọc ra các guideIds mà nằm trong thời gian tiến hành 
            var busyGuideIds = await _context.Guides.Where(g => activeScheduleIds.Contains(g.ScheduleId))
            .Select(g => g.StaffId)
            .ToArrayAsync();

            // lọc ra các staff là tourguide và không nằm trong khoảng thời gian bận
            var availableTourGuides = await _context.Staffs.Include(s => s.User)
            .ThenInclude(u => u!.Role)
            .Where(s => s.User!.RoleId == 2 && !busyGuideIds.Contains(s.UserId))
            .Select(s => s.Map())
            .AsNoTracking()
            .ToArrayAsync();

            return Ok(new RestDTO<StaffDTO[]?>()
            {
                Data = availableTourGuides
            });
        }

        [HttpGet("{id}")]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> GetStaffById(int id)
        {
            var staff = await _context.Staffs.Include(s => s.User).ThenInclude(u => u!.Role).AsNoTracking().FirstOrDefaultAsync(s => s.UserId == id);

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

                _context.Staffs.Add(staff);
                await _context.SaveChangesAsync();


                return Ok(new RestDTO<int>()
                {
                    Data = staff.UserId
                });
            }
            catch (Exception ex)
            {
                return Problem("Error creating the staff: " + ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateStaff(UpdateStaffDTO updatedStaffDTO)
        {
            try
            {
                var staff = await _context.Staffs.Include(s => s.User).FirstOrDefaultAsync(s => s.UserId == updatedStaffDTO.UserId);

                if (staff == null)
                {
                    return NotFound($"id {updatedStaffDTO.UserId} not found");
                }

                updatedStaffDTO.UpdateEntity(staff);
                await _context.SaveChangesAsync();

                return Ok(new RestDTO<Boolean>()
                {
                    Data = true
                });
            }
            catch (Exception ex)
            {
                return Problem("Error updating the staff: " + ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStaff(int id)
        {
            try
            {
                var staff = await _context.Staffs.FindAsync(id);
                if (staff == null)
                {
                    return NotFound($"id {id} not found");
                }

                _context.Staffs.Remove(staff);
                await _context.SaveChangesAsync();

                return Ok(new RestDTO<Boolean>()
                {
                    Data = true
                });
            }
            catch (Exception ex)
            {
                return Problem("Error deleting the staff: " + ex.Message);
            }
        }
    }
}