using BookingTravelApi.Domains;
using BookingTravelApi.DTO;
using BookingTravelApi.DTO.Location;
using BookingTravelApi.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingTravelApi.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class LocationController : Controller
    {
        private ApplicationDbContext _context;
        private readonly ILogger<LocationController> _logger;

        public LocationController(ILogger<LocationController> logger, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any)]
        public async Task<IActionResult> getLocations(String? filter = null)
        {
            var query = _context.Locations.AsQueryable();

            var searchStr = filter?.Trim();
            if (!String.IsNullOrEmpty(searchStr))
            {
                query.Where(i => i.Name.Contains(searchStr));
            }
            Location[] locations = await _context.Locations.ToArrayAsync();
            LocationDTO[] locationDTOs = locations.Select(location => location.Map()).ToArray();

            return Ok(new RestDTO<LocationDTO[]?>
            {
                Data = locationDTOs
            });
        }
    }
}
