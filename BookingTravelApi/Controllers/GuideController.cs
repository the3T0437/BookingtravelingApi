using System.Linq.Dynamic.Core;
using BookingTravelApi.Domains;
using BookingTravelApi.DTO;
using BookingTravelApi.DTO.Activity;
using BookingTravelApi.DTO.guide;
using BookingTravelApi.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingTravelApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GuideController : Controller
    {
        private ApplicationDbContext _context;
        private readonly ILogger<GuideController> _logger;

        public GuideController(ILogger<GuideController> logger, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("ByStaff/{staffId}")]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> getGuidesStaffId(int? staffId = null)
        {
            try
            {
                if (staffId == null)
                {
                    return Problem("id not found");
                }

                var query = await _context.Guides
                .Where(g => g.StaffId == staffId)
                .Include(t => t.Schedule)
                .ThenInclude(s => s!.Tour)
                .ThenInclude(t => t!.TourLocations!)

                .Include(t => t.Schedule)
                .ThenInclude(s => s!.UserCompletedSchedules)
                .AsNoTracking().ToListAsync();

                var guideDTOs = query.Select(i => i.Map()).ToArray();


                return Ok(new RestDTO<GuideDTO[]?>()
                {
                    Data = guideDTOs
                });
            }
            catch(Exception ex)
            {
                // ✅ Log chi tiết exception
        Console.WriteLine($"Error: {ex.Message}");
        Console.WriteLine($"Inner: {ex.InnerException?.Message}");
        Console.WriteLine($"Stack: {ex.StackTrace}");
        return Problem($"ERROR: {ex.Message}");
            }
        }

        [HttpGet("BySchedule/{scheduleId}")]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> getGuidesScheduleId(int? scheduleId = null)
        {
            try
            {
                if (scheduleId == null)
                {
                    return Problem("id not found");
                }
                var query = _context.Guides
                .Where(g => g.ScheduleId == scheduleId)
                .Include(g => g.Staff)
                !.ThenInclude(us => us!.User)

                .Include(g => g.Schedule)
                .ThenInclude(s => s!.Tour)
                .ThenInclude(t => t!.TourLocations)
                !.ThenInclude(tl => tl.Location)
                !.AsNoTracking();


                var guideDTOs = await query.Select(i => i.Map()).ToArrayAsync();

                return Ok(new RestDTO<GuideDTO[]?>()
                {
                    Data = guideDTOs
                });
            }
            catch (Exception ex)
            {
                return Problem($"ERROR GuidesscheduleId");
            }
        }

        [HttpPost(Name = "CreateGuide")]
        public async Task<IActionResult> createGuide(CreateGuideDTO newGuideDTO)
        {
            try
            {
                var guide = newGuideDTO.Map();

                await _context.Guides.AddAsync(guide);
                await _context.SaveChangesAsync();

                return Ok(new RestDTO<int>()
                {
                    Data = guide.StaffId
                });
            }
            catch (Exception ex)
            {
                return Problem("Error create");
            }
        }

        [HttpDelete(Name = "DeleteGuide")]
        public async Task<IActionResult> deleteGuide(int staffId, int scheduleId)
        {
            try
            {
                var guide = await _context.Guides.Where(g => g.StaffId == staffId && g.ScheduleId == scheduleId).FirstOrDefaultAsync();

                if (guide == null)
                {
                    return NotFound($"Place with Id {staffId} or {scheduleId} not found.");
                }

                _context.Guides.Remove(guide);
                await _context.SaveChangesAsync();

                return Ok(new RestDTO<Boolean>()
                {
                    Data = true
                });
            }
            catch (Exception ex)
            {
                return Problem("Error delete");
            }
        }

    }
}