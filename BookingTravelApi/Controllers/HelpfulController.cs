using System.Linq.Dynamic.Core;
using BookingTravelApi.Domains;
using BookingTravelApi.DTO;
using BookingTravelApi.DTO.Activity;
using BookingTravelApi.DTO.guide;
using BookingTravelApi.DTO.Helpful;
using BookingTravelApi.DTO.TourGuide;
using BookingTravelApi.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingTravelApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HelpfulController : Controller
    {
        private ApplicationDbContext _context;
        private readonly ILogger<GuideController> _logger;

        public HelpfulController(ILogger<GuideController> logger, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost(Name = "helpful")]
        public async Task<IActionResult> ClickHelpful(ClickHelpfulRequest request)
        {

            Helpful? helpful = null;
            helpful = await _context.Helpfuls.Where(i => i.UserId == request.UserId && i.ReviewId == request.ReviewId).FirstOrDefaultAsync();

            if (helpful == null)
            {
                helpful = request.Map();
                return await CreateHelpful(helpful);
            }

            return await RemoveHelpful(helpful);
        }

        private async Task<IActionResult> RemoveHelpful(Helpful helpful)
        {
            _context.Helpfuls.Remove(helpful);
            await _context.SaveChangesAsync();
            var result = helpful.Map();
            result.isEnable = false;

            return Ok(new RestDTO<HelpfulDTO>()
            {
                Data = result
            });
        }

        private async Task<IActionResult> CreateHelpful(Helpful helpful)
        {
            await _context.Helpfuls.AddAsync(helpful);
            await _context.SaveChangesAsync();
            var result = helpful.Map();
            result.isEnable = true;

            return Ok(new RestDTO<HelpfulDTO>()
            {
                Data = result
            });
        }
    }
}