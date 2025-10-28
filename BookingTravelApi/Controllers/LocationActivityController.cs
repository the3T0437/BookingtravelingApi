using System.Linq.Dynamic.Core;
using BookingTravelApi.Domains;
using BookingTravelApi.DTO;
using BookingTravelApi.DTO.LocationActivity;
using BookingTravelApi.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingTravelApi.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class LocationActivityController : Controller
    {
        private ApplicationDbContext _context;
        private readonly ILogger<LocationController> _logger;

        public LocationActivityController(ILogger<LocationController> logger, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet(Name = "GetLocationActivities")]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> getLocationActivities(int placeId, String orderBy = "Name", String sortBy = "ASC", String? filter = null)
        {
            var query = _context.LocationActivities.AsQueryable();
            query = query.Where(i => i.PlaceId == placeId);
            if (!String.IsNullOrEmpty(filter))
            {
                query = query.Where(locationActivity => locationActivity.Name.Contains(filter));
            }
            query = query.OrderBy($"{orderBy} {sortBy}");

            var locationsActivities = await query.Include(i => i.Place)
                .ThenInclude(i => i.Location)
                .Include(i => i.ActivityAndLocations)
                !.ThenInclude(i => i.Activity)
                .ToListAsync();

            var locationActivityDTOs = locationsActivities.Select(i => i.Map()).ToList();

            return Ok(new RestDTO<List<LocationActivityDTO>>()
            {
                Data = locationActivityDTOs
            });
        }

        [HttpGet("{id:int}", Name = "GetLocationActivity")]
        public async Task<IActionResult> getLocationActivity(int id)
        {
            var locationActivity = await _context.LocationActivities
                    .Include(i => i.ActivityAndLocations)
                    !.ThenInclude(i => i.Activity)
                    .Include(i => i.Place)
                    .ThenInclude(i => i.Location)
                    .Where(i => i.Id == id)
                    .FirstOrDefaultAsync();
            if (locationActivity == null)
            {
                return NotFound(new
                {
                    message = "not found location activity"
                });
            }

            return Ok(new RestDTO<LocationActivityDTO>()
            {
                Data = locationActivity.Map()
            });
        }


        [HttpPost(Name = "CreateLocationActivity")]
        public async Task<IActionResult> CreateLocationActivity(CreateLocationActivityDTO newLocationActivityDTO)
        {
            try
            {
                var newLocationActivity = newLocationActivityDTO.Map();
                await _context.LocationActivities.AddAsync(newLocationActivity);
                await _context.SaveChangesAsync();

                return Ok(new RestDTO<int>()
                {
                    Data = newLocationActivity.Id
                });
            }
            catch (Exception e)
            {
                return BadRequest(
                    new
                    {
                        message = $"{e.Message}",
                        data = $"{e.InnerException?.Message}",
                    }
                );
            }
        }

        [HttpPut(Name = "UpdateLocationActivity")]
        public async Task<IActionResult> UpdateLocationActivity(UpdateLocationActivityDTO updateLocationActivity)
        {
            var locationActivity = await _context.LocationActivities.Include(i => i.ActivityAndLocations).Where(i => i.Id == updateLocationActivity.Id).FirstOrDefaultAsync();

            if (locationActivity != null)
            {
                if (updateLocationActivity.PlaceId != null)
                {
                    locationActivity.PlaceId = updateLocationActivity.PlaceId.Value;
                }

                if (!String.IsNullOrEmpty(updateLocationActivity.Name))
                {
                    locationActivity.Name = updateLocationActivity.Name;
                }

                if (updateLocationActivity.ActivityIds != null)
                {
                    _context.ActivityAndLocations.RemoveRange(locationActivity.ActivityAndLocations!);
                    var locationAndActivities = updateLocationActivity.ActivityIds.Select(i => new ActivityAndLocation() { ActivityId = i }).ToList();
                    locationActivity.ActivityAndLocations = locationAndActivities;
                }

                _context.Update(locationActivity);
                await _context.SaveChangesAsync();

                return await getLocationActivity(updateLocationActivity.Id);
            }

            return NotFound(new
            {
                Message = "location activity not found"
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> deleteLocationActivity(int id)
        {
            var locationsActivity = await _context.LocationActivities.Include(i => i.Place).ThenInclude(i => i.Location).Where(i => i.Id == id).FirstOrDefaultAsync();
            if (locationsActivity == null)
            {
                return NotFound(new ErrorDTO("Not found"));
            }

            _context.LocationActivities.Remove(locationsActivity);
            await _context.SaveChangesAsync();

            return Ok(new RestDTO<LocationActivityDTO>()
            {
                Data = locationsActivity.Map()
            });
        }
    }
}
