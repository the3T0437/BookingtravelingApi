using System.Linq.Dynamic.Core;
using BookingTravelApi.Domains;
using BookingTravelApi.DTO;
using BookingTravelApi.DTO.Location;
using BookingTravelApi.DTO.LocationActivity;
using BookingTravelApi.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
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

            var locationsActivities = await query.Include(i => i.Place).ToListAsync();
            var locationActivityDTOs = locationsActivities.Select(i => i.Map()).ToList();

            return Ok(new RestDTO<List<LocationActivityDTO>>()
            {
                Data = locationActivityDTOs
            });
        }

        [HttpGet("{id:int}", Name = "GetLocationActivity")]
        public async Task<IActionResult> getLocationActivity(int id)
        {
            var locationActivity = await _context.LocationActivities.Where(i => i.Id == id).FirstOrDefaultAsync();
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

            // var check = await CheckActivityIds(newLocationActivityDTO.ActivityIds);
            // if (check != null)
            // {
            //     return check;
            // }

            var newLocationActivity = newLocationActivityDTO.Map();
            newLocationActivity.ActivityAndLocations = newLocationActivityDTO.ActivityIds?.Select(i => new ActivityAndLocation() { ActivityId = i }).ToList();
            await _context.LocationActivities.AddAsync(newLocationActivity);
            await _context.SaveChangesAsync();

            // foreach (int i in newLocationActivityDTO.ActivityIds)
            // {
            //     var link = new ActivityAndLocation()
            //     {
            //         ActivityId = i,
            //         LocationActivityId = newLocationActivity.Id
            //     };
            //     await _context.ActivityAndLocations.AddAsync(link);
            // }
            // await _context.SaveChangesAsync();

            return Ok(new RestDTO<LocationActivityDTO>()
            {
                Data = newLocationActivity.Map()
            });
        }

        [HttpPut(Name = "UpdateLocationActivity")]
        public async Task<IActionResult> UpdateLocationActivity(UpdateLocationActivityDTO updateLocationActivity)
        {
            var check = await CheckActivityIds(updateLocationActivity.ActivityIds);
            if (check != null)
            {
                return check;
            }

            var locationActivity = await _context.LocationActivities.Where(i => i.Id == updateLocationActivity.Id).FirstOrDefaultAsync();

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
                    var items = _context.ActivityAndLocations.Where(i => i.LocationActivityId == updateLocationActivity.Id);
                    _context.RemoveRange(items);

                    var newItems = updateLocationActivity.ActivityIds.Select(
                        i => new ActivityAndLocation() { ActivityId = i, LocationActivityId = updateLocationActivity.Id }
                    );
                    await _context.AddRangeAsync(newItems);
                }

                _context.Update(locationActivity);
                await _context.SaveChangesAsync();

                return Ok(new RestDTO<LocationActivityDTO>()
                {
                    Data = locationActivity.Map()
                });
            }

            return NotFound(new
            {
                Message = "location activity not found"
            });
        }

        private async Task<IActionResult?> CheckActivityIds(List<int> activityIds)
        {
            var existActivities = await _context.Activities
                .Where(i => activityIds.Contains(i.Id))
                .Select(i => i.Id)
                .ToListAsync();
            var missingActivities = existActivities.Except(activityIds);

            if (missingActivities.Any())
            {
                return BadRequest(
                    new
                    {
                        Message = "Some activity missing",
                        Data = missingActivities
                    }
                );
            }

            return null;
        }
    }
}
