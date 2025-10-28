using System.Linq.Dynamic.Core;
using BookingTravelApi.Domains;
using BookingTravelApi.DTO;
using BookingTravelApi.DTO.Activity;
using BookingTravelApi.DTO.guide;
using BookingTravelApi.DTO.TourGuide;
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

        [HttpPost("{scheduleId}")]
        public async Task<IActionResult> checkGuideAssignment([FromRoute] int scheduleId, [FromBody] List<TourGuideDTO> tourGuides)
        {
            try
            {
                // lấy các guide ra
                var guides = await _context.Guides.Where(g => g.ScheduleId == scheduleId).ToListAsync();

                // Lọc ra các id guides để duyệt
                var guideIds = guides.Select(g => g.StaffId).ToHashSet();

                foreach (var item in tourGuides)
                {
                    if (guideIds.Contains(item.UserId) && item.ischecked == false)
                    {
                        var guideToRemove = guides.FirstOrDefault(g => g.StaffId == item.UserId);

                        if (guideToRemove != null)
                        {
                            _context.Guides.Remove(guideToRemove);
                        }
                    }
                    else if (!guideIds.Contains(item.UserId) && item.ischecked == true)
                    {
                        var guide = new CreateGuideDTO(item.UserId, scheduleId);
                        _context.Guides.Add(guide.Map());
                    }
                }

                await _context.SaveChangesAsync();

                return Ok(new RestDTO<bool>()
                {
                    Data = true
                });

            }
            catch (Exception e)
            {
                return Problem($"ERRO {e.Message}");
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