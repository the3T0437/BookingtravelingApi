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


                var result = await _context.Staffs.Include(s => s.User).ThenInclude(u => u.Role).FirstAsync(s => s.UserId == staff.UserId);


                return Ok(new RestDTO<StaffDTO?>()
                {
                    Data = result.Map()
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
                var staff = await _context.Staffs.Where(s => s.UserId == updatedStaffDTO.UserId).Include(s => s.User).FirstOrDefaultAsync();

                if (staff == null)
                {
                    return NotFound($"id {updatedStaffDTO.UserId} not found");
                }

                updatedStaffDTO.UpdateEntity(staff);

                await _context.SaveChangesAsync();

                var result = await _context.Staffs.Include(s => s.User).ThenInclude(u => u.Role).FirstAsync(s => s.UserId == staff.UserId);

                return Ok(new RestDTO<StaffDTO?>()
                {
                    Data = result.Map()
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

                return Ok(new RestDTO<string?>()
                {
                    Data = $"Staff with id {id} has been deleted."
                });
            }
            catch (Exception ex)
            {
                return Problem("Error deleting the staff: " + ex.Message);
            }
        }
    }
}