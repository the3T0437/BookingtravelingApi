using System.Linq.Dynamic.Core;
using BookingTravelApi.Domains;
using BookingTravelApi.DTO;
using BookingTravelApi.DTO.AssignmentDTO;
using BookingTravelApi.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingTravelApi.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class AssignmentController : Controller
    {
        private ApplicationDbContext _context;
         private readonly ILogger<LocationController> _logger;

        public AssignmentController(ILogger<LocationController> logger, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> getSchedules()
        {
            var now = DateTime.Now;

            var query = _context.Schedules
            .Include(s => s.Tour)
            .ThenInclude(t => t.TourImages)

            .Include(s => s.Tour)
            .ThenInclude(t => t.TourLocations!)
            .ThenInclude(tl => tl.Location)

            .Include(s => s.Tour)
            .ThenInclude(t => t.TourLocations!)
            .ThenInclude(tl => tl.Location!)
            .ThenInclude(l => l.Places)

            .Where(s => now <= s.OpenDate)
            .AsNoTracking();

            var assignmentDTOs = await query.Select(i => i.MapAssignment()).ToArrayAsync();

            return Ok(new RestDTO<AssignmentDTO[]?>
            {
                Data = assignmentDTOs
            });
        }
    }
}