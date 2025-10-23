using System.Linq.Dynamic.Core;
using BookingTravelApi.DTO;
using BookingTravelApi.DTO.schedule;
using BookingTravelApi.Domains;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookingTravelApi.Extensions;

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

        
        [HttpGet]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> getSchedules()
        {
            var query = _context.Schedules
            .Include(s => s.Tour)
            .ThenInclude(t => t.TourImages)

            .Include(s => s.Tour)
            .ThenInclude(t => t.TourLocations!)
            .ThenInclude(tl => tl.Location)
            
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

            .Include(s => s.Tour)
            .ThenInclude(t => t.TourLocations!)
            .ThenInclude(tl => tl.Location)

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

        [HttpGet("SchedulesUser")]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> getSchedulesUser()
        {

            var now = DateTime.Now;

            var query = _context.Schedules
            .Include(s => s.Tour)
            .ThenInclude(t => t.TourImages)

            .Include(s => s.Tour)
            .ThenInclude(t => t.TourLocations!)
            .ThenInclude(tl => tl.Location)
              
            .Where(s => s.OpenDate <= now && s.StartDate > now && s.MaxSlot > 0)
            .AsNoTracking();

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