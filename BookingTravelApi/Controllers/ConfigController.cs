using BookingTravelApi.Domains;
using BookingTravelApi.DTO;
using BookingTravelApi.DTO.config;
using BookingTravelApi.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingTravelApi.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class ConfigController : Controller
    {
        private ApplicationDbContext _context;
        private readonly ILogger<ConfigController> _logger;

        public ConfigController(ILogger<ConfigController> logger, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> getConfigs()
        {
            var configs = await _context.Configs.AsNoTracking().Select(c => c.Map()).ToArrayAsync();

            return Ok(new RestDTO<ConfigDTO[]?>()
            {
                Data = configs
            });
        }
    }
}