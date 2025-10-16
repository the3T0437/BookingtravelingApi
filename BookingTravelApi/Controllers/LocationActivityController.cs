using System.Linq.Dynamic.Core;
using BookingTravelApi.Domains;
using BookingTravelApi.DTO;
using BookingTravelApi.DTO.Location;
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
        public async Task<RestDTO<LocationActivityDTO[]>> getLocationActivities(String orderBy = "Name", String sortBy = "ASC", String? filter = null)
        {
            var query = _context.LocationActivities.AsQueryable();
            if (!String.IsNullOrEmpty(filter))
            {
                query = query.Where(locationActivity => locationActivity.Name.Contains(filter));
            }
            query = query.OrderBy($"{orderBy} {sortBy}");

            var locationsActivities = await query.ToArrayAsync();
            var locationActivityDTOs = locationsActivities.Select(i => i.Map()).ToArray();

            return new RestDTO<LocationActivityDTO[]>()
            {
                Data = locationActivityDTOs
            };
        }

        [HttpPost(Name = "CreateLocationActivity")]
        public async Task<RestDTO<LocationActivityDTO>> CreateLocationActivity(CreateLocationActivityDTO newLocationActivityDTO)
        {
            var newLocationActivity = newLocationActivityDTO.Map();
            await _context.LocationActivities.AddAsync(newLocationActivity);
            await _context.SaveChangesAsync();

            foreach (int i in newLocationActivityDTO.ActivityIds)
            {
                var link = new ActivityAndLocation()
                {
                    ActivityId = i,
                    LocationActivityId = newLocationActivity.Id
                };
                await _context.ActivityAndLocations.AddAsync(link);
            }
            await _context.SaveChangesAsync();


            return new RestDTO<LocationActivityDTO>()
            {
                Data = newLocationActivity.Map()
            };
        }

        [HttpPut(Name = "UpdateLocationActivity")]
        public async Task<RestDTO<LocationActivityDTO?>> UpdateLocationActivity(UpdateLocationActivityDTO updateLocationActivity)
        {
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
            }

            return new RestDTO<LocationActivityDTO?>()
            {
                Data = locationActivity?.Map()
            };
        }
    }
}
