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
    [Route("[Controller]")]
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

        [HttpGet]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> getGuides(String orderBy = "StaffId", String sortBy = "ASC", int? staffId = null, int? scheduleId = null)
        {
            var query = _context.Guides.AsQueryable();

            if (staffId != null)
            {
                query = query.Where(g => g.StaffId == staffId);
            }

            if (scheduleId != null)
            {
                query = query.Where(g => g.ScheduleId == scheduleId);
            }

            query = query.OrderBy($"{orderBy} {sortBy}");

            var guides = await query.ToArrayAsync();
            var placeDTOs = guides.Select(g => g.Map()).ToArray();

            return Ok(new RestDTO<GuideDTO[]?>()
            {
                Data = placeDTOs
            });
        }

        [HttpPost(Name = "CreateGuide")]
        public async Task<IActionResult> CreateGuide(CreateGuideDTO newGuideDTO)
        {
            try
            {
                var guide = newGuideDTO.Map();

                await _context.Guides.AddAsync(guide);
                await _context.SaveChangesAsync();


                // Trả về HTTP 200
                return Ok(new RestDTO<GuideDTO?>()
                {
                    Data = guide?.Map()
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
                var guide = await _context.Guides.Where(g => g.StaffId == staffId  && g.ScheduleId == scheduleId).FirstOrDefaultAsync();

                if (guide == null)
                {
                    // Trả về HTTP 404 Not Found
                    return NotFound($"Place with Id {staffId} or {scheduleId} not found.");
                }

                _context.Guides.Remove(guide);
                await _context.SaveChangesAsync();

                // Trả về HTTP 200
                return Ok(new RestDTO<GuideDTO?>()
                {
                    Data = guide?.Map()
                });
            }
            catch (Exception ex)
            {
                return Problem("Error delete");
            }
        }

    }
}