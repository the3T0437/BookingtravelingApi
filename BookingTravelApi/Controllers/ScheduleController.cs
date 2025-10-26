using System.Linq.Dynamic.Core;
using BookingTravelApi.DTO;
using BookingTravelApi.DTO.schedule;
using BookingTravelApi.Domains;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookingTravelApi.Extensions;
using BookingTravelApi.DTO.ScheduleAssignmentDTO;

namespace BookingTravelApi.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class ScheduleController : Controller
    {
        private ApplicationDbContext _context;
        private readonly ILogger<LocationController> _logger;

        public ScheduleController(ILogger<LocationController> logger, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("assignment")]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> getScheduleAssignment()
        {

            var timeNow = DateTime.Now;
            var scheduleDTOs = await _context.Schedules
            .OrderByDescending(s => s.OpenDate)
            .Select(s => new ScheduleAssignmentDTO
            {
                idSchedule = s.Id,
                titleTour = s.Tour!.Title,
                tourImages = s.Tour.TourImages!.Select(it => it.Path).ToList(),
                nameLocations = s.Tour.TourLocations!.Select(it => it.Location!.Name).ToList(),
                placeNames = s.Tour.TourLocations!
                .SelectMany(it => it.Location!.Places!)
                .Select(p => p.Name)
            .ToList()
            })
            .AsNoTracking()
            .ToArrayAsync();

            return Ok(new RestDTO<ScheduleAssignmentDTO[]?>()
            {
                Data = scheduleDTOs
            });
        }


        [HttpGet("assignment/{idtour}")]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> getScheduleAssignmentByIdTour(int idtour)
        {
            var query = _context.Schedules.Where(s => s.TourId == idtour)
            .Include(s => s.Tour)
            .OrderByDescending(s => s.OpenDate).AsNoTracking();

            var scheduleDTOs = await query.Select(i => i.Map()).ToArrayAsync();

            return Ok(new RestDTO<ScheduleDTO[]?>()
            {
                Data = scheduleDTOs
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetScheduleById(int id)
        {
            var schedule = await _context.Schedules
            .Include(s => s.Tour)
            .ThenInclude(t => t.TourImages)

            .Include(i => i.Tour)
            .ThenInclude(tm => tm.DayOfTours!)
            .ThenInclude(d => d.DayActivities!)
            .ThenInclude(da => da.Activity)

            .Include(i => i.Tour)
            .ThenInclude(tm => tm.DayOfTours!)
            .ThenInclude(i => i.DayActivities!)
            .ThenInclude(i => i.LocationActivity)
            .ThenInclude(i => i!.Place)
            .ThenInclude(i => i!.Location)

            .OrderByDescending(s => s.OpenDate).AsNoTracking()

            .FirstOrDefaultAsync(s => s.Id == id);


            if (schedule == null)
            {
                return NotFound($"id {id} not found");
            }

            return Ok(new RestDTO<ScheduleDTO?>()
            {
                Data = schedule.Map()
            });
        }

        [HttpGet("schedules-user")]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> getSchedulesUser()
        {

            var now = DateTime.Now;

            var query = _context.Schedules
            .Include(s => s.Tour)
            .ThenInclude(t => t.TourImages)

            .Include(i => i.Tour)
            .ThenInclude(tm => tm.DayOfTours!)
            .ThenInclude(d => d.DayActivities!)
            .ThenInclude(da => da.Activity)

            .Include(i => i.Tour)
            .ThenInclude(tm => tm.DayOfTours!)
            .ThenInclude(i => i.DayActivities!)
            .ThenInclude(i => i.LocationActivity)
            .ThenInclude(i => i!.Place)
            .ThenInclude(i => i!.Location)

            .OrderByDescending(s => s.OpenDate).AsNoTracking()

            .Where(s => s.OpenDate <= now && s.StartDate > now && s.MaxSlot > 0);

            var scheduleDTOs = await query.Select(i => i.Map()).ToArrayAsync();


            return Ok(new RestDTO<ScheduleDTO[]?>()
            {
                Data = scheduleDTOs
            });
        }



        [HttpPost(Name = "CreateSchedule")]
        public async Task<IActionResult> CreateSchedule(CreateScheduleDTO newScheduleDTO)
        {
            try
            {
                var schedule = newScheduleDTO.Map();

                _context.Schedules.Add(schedule);
                await _context.SaveChangesAsync();

                // var scheduleDTOid = _context.Schedules.fin

                return Ok(new RestDTO<int>()
                {
                    Data = schedule.Id
                });
            }
            catch (Exception ex)
            {
                return Problem("Error creating schedule: " + ex.Message);
            }

        }


        [HttpPut(Name = "UpdateSchedule")]
        public async Task<IActionResult> UpdateSchedule(UpdateScheduleDTO updatedSchedule)
        {
            try
            {
                var schedule = await _context.Schedules.FirstOrDefaultAsync(s => s.Id == updatedSchedule.Id);

                if (schedule == null)
                {
                    return NotFound($"Id {updatedSchedule.Id} not found.");
                }

                updatedSchedule.UpdateEntity(schedule);

                await _context.SaveChangesAsync();

                return Ok(new RestDTO<Boolean>()
                {
                    Data = true
                });
            }
            catch (Exception ex)
            {
                return Problem("Error updating schedule: " + ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSchedule(int id)
        {
            try
            {
                var schedule = await _context.Schedules.FindAsync(id);
                if (schedule == null)
                {
                    return NotFound($"Id {id} not found.");
                }

                _context.Schedules.Remove(schedule);
                await _context.SaveChangesAsync();

                return Ok(new RestDTO<bool>()
                {
                    Data = true
                });
            }
            catch (Exception ex)
            {
                return Problem("Error deleting schedule: " + ex.Message);
            }
        }
    }
}