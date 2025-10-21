using System.Linq.Dynamic.Core;
using BookingTravelApi.Domains;
using BookingTravelApi.DTO;
using BookingTravelApi.DTO.Activity;
using BookingTravelApi.DTO.Location;
using BookingTravelApi.DTO.place;
using BookingTravelApi.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingTravelApi.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class ActivityController : Controller
    {
        private ApplicationDbContext _context;
        private readonly ILogger<LocationController> _logger;

        public ActivityController(ILogger<LocationController> logger, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet(Name = "GetActivity")]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> GetActivities(
            int? locationActivityId = null,
            String orderBy = "Action",
            String sortBy = "ASC",
            String? filter = null
            )
        {
            try
            {
                var query = _context.Activities.AsQueryable();
                var searchStr = filter?.Trim();
                if (locationActivityId != null)
                {
                    query = query.Where((activity) => activity.ActivityAndLocations
                            .Select(i => i.LocationActivityId)
                            .Contains(locationActivityId.Value));
                }
                if (!String.IsNullOrEmpty(searchStr))
                {
                    query = query.Where(place => place.Action.Contains(searchStr));
                }
                query = query.OrderBy($"{orderBy} {sortBy}").AsQueryable();

                var activities = await query.ToArrayAsync();
                var activityDTOs = activities.Select(place => place.Map()).ToArray();

                return Ok(new RestDTO<ActivityDTO[]>()
                {
                    Data = activityDTOs
                });
            }
            catch (Exception e)
            {
                return Problem(
                    e.Message
                );
            }
        }

        [HttpPost(Name = "CreateActivity")]
        public async Task<IActionResult> CreateActivity(CreateActivityDTO createActivityDTO)
        {
            try
            {
                var activity = createActivityDTO.Map();
                await _context.Activities.AddAsync(activity);
                await _context.SaveChangesAsync();

                return Ok(new RestDTO<ActivityDTO>()
                {
                    Data = activity.Map()
                });
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        [HttpPut(Name = "UpdateActivity")]
        public async Task<IActionResult> UpdateActivity(UpdateActivityDTO updateActivityDTO)
        {
            try
            {
                var activity = await _context.Activities.Where(place => place.Id == updateActivityDTO.Id).FirstOrDefaultAsync();

                if (activity == null)
                {
                    return NotFound();
                }

                if (updateActivityDTO.Action != null)
                {
                    activity.Action = updateActivityDTO.Action;
                }

                _context.Activities.Update(activity);
                await _context.SaveChangesAsync();

                return Ok(new RestDTO<ActivityDTO>()
                {
                    Data = activity.Map()
                });

            }
            catch (Exception e)
            {
                return NotFound(new
                {
                    message = "activity not found"
                });
            }

        }
    }
}
