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

        [HttpGet("schedule/{staffId}")]
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
                .ThenInclude(s => s!.UserCompletedSchedules)
                !.ThenInclude(s => s.User)

                .Include(t => t.Schedule)
                .ThenInclude(s => s!.Bookings)

                // new
                .Include(t => t.Staff)
                .ThenInclude(s => s!.User)
                
                .Include(t => t.Schedule!.Tour!.TourImages)
                .AsNoTracking().ToListAsync();

                var guideDTOs = query.Select(i => i.Map()).ToArray();

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
                    return NotFound($"Guide with Id {staffId} or {scheduleId} not found.");
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