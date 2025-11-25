using System.Linq.Dynamic.Core;
using BookingTravelApi.DTO;
using BookingTravelApi.DTO.schedule;
using BookingTravelApi.Domains;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookingTravelApi.Extensions;
using BookingTravelApi.DTO.ScheduleAssignmentDTO;
using Org.BouncyCastle.Asn1.Cms;
using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore.Query;
using System.ComponentModel.DataAnnotations;

namespace BookingTravelApi.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class ScheduleController : Controller
    {
        private ApplicationDbContext _context;
        private readonly ILogger<ScheduleController> _logger;

        public ScheduleController(ILogger<ScheduleController> logger, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        // màn 29
        [HttpGet]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> getSchedules()
        {

            var query = _context.Schedules
            .Include(i => i.Bookings)

            .Include(s => s.Tour)
            .ThenInclude(t => t.TourImages)

            .Include(i => i.Tour)
            .ThenInclude(tm => tm.DayOfTours!)
            .ThenInclude(i => i.DayActivities!)
            .ThenInclude(i => i.LocationActivity)
            .ThenInclude(i => i!.Place)
            .ThenInclude(i => i!.Location)

            .OrderByDescending(s => s.OpenDate).AsNoTracking();

            var scheduleDTOs = await query.Select(i => i.Map()).ToArrayAsync();

            return Ok(new RestDTO<ScheduleDTO[]?>()
            {
                Data = scheduleDTOs
            });
        }

        // Màn 36
        [HttpGet("assignment/{tourId}")]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> getScheduleAssignment(int tourId)
        {
            var timeNow = DateTime.UtcNow.AddHours(7);

            // lấy ra các schedule có open date trong tương lai
            var query = _context.Schedules.Where(
                s => s.OpenDate >= timeNow
            ).Where(s => s.TourId == tourId).OrderByDescending(s => s.OpenDate).AsNoTracking();

            // Lấy các ScheduleIds đã có người hướng dẫn
            var guideScheduleIds = await _context.Guides
                .Select(g => g.ScheduleId)
                .ToHashSetAsync();

            var scheduleDTOs = await query.Select(s => new ScheduleAssignmentDTO
            {
                Id = s.Id,
                Code = s.Code,
                StartDate = s.StartDate,
                EndDate = s.EndDate,
                isAssignment = guideScheduleIds.Contains(s.Id)
            }).ToArrayAsync();

            return Ok(new RestDTO<ScheduleAssignmentDTO[]?>()
            {
                Data = scheduleDTOs
            });
        }

        // màn 37
        [HttpGet("tour/{idtour}")]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> getScheduleAssignmentByIdTour(int idtour)
        {
            var timeNow = DateTime.UtcNow.AddHours(7);

            var query = _context.Schedules
                .Where(s => s.TourId == idtour)
                .Where(
                    s => s.OpenDate <= timeNow && timeNow <= s.StartDate
                )
                // .Where(
                //     s => timeNow <= s.StartDate
                // )
                .Include(s => s.Bookings)

                .Include(s => s.Tour)
                .ThenInclude(t => t!.TourImages)

                .Include(i => i.Tour)
                .ThenInclude(tm => tm!.DayOfTours!)
                .ThenInclude(d => d.DayActivities!)
                .ThenInclude(da => da.Activity)

                .Include(i => i.Tour)
                .ThenInclude(tm => tm!.DayOfTours!)
                .ThenInclude(i => i.DayActivities!)
                .ThenInclude(i => i.LocationActivity)
                .ThenInclude(i => i!.Place)
                .ThenInclude(i => i!.Location)
                    .OrderBy(s => s.OpenDate).AsNoTracking();

            var scheduleDTOs = await query.Select(i => i.MapToScheduleOfAccountant()).ToArrayAsync();

            return Ok(new RestDTO<ScheduleDTOOfAccountant[]?>()
            {
                Data = scheduleDTOs
            });
        }

        // màn 37
        [HttpGet("{id}")]
        public async Task<IActionResult> GetScheduleById(int id)
        {
            var schedule = await _context.Schedules
            .Include(i => i.Bookings)

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

        [HttpGet("Accountant")]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> GetSchedulesForAccountant()
        {

            var now = DateTime.UtcNow.AddHours(7);

            var query = _context.Schedules
                .Where(
                    s => s.OpenDate < now && now < s.EndDate.AddDays(1)
                )
                .Include(s => s.Bookings)
                !.ThenInclude(s => s.Status)

                .Include(s => s.Tour)
                .ThenInclude(t => t!.TourImages)

                .Include(i => i.Tour)
                .ThenInclude(tm => tm!.DayOfTours!)
                .ThenInclude(d => d.DayActivities!)
                .ThenInclude(da => da.Activity)

                .Include(i => i.Tour)
                .ThenInclude(tm => tm!.DayOfTours!)
                .ThenInclude(i => i.DayActivities!)
                .ThenInclude(i => i.LocationActivity)
                .ThenInclude(i => i!.Place)
                .ThenInclude(i => i!.Location)
                    .OrderBy(s => s.OpenDate).AsNoTracking();

            var scheduleDTOs = await query.Select(i => i.MapToScheduleOfAccountant()).ToArrayAsync();


            return Ok(new RestDTO<ScheduleDTOOfAccountant[]?>()
            {
                Data = scheduleDTOs
            });
        }

        [HttpGet("Reception")]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> GetSchedulesForReception()
        {

            var now = DateTime.UtcNow.AddHours(7);
            var startDate = new DateTime(now.Year, now.Month, now.Day);
            var endDate = startDate.AddDays(1);

            var query = _context.Schedules
                .Where(
                    s => startDate <= s.StartDate && s.StartDate < endDate
                )
                .Include(i => i.Bookings)

                .Include(s => s.Tour)
                .ThenInclude(t => t!.TourImages)

                .Include(i => i.Tour)
                .ThenInclude(tm => tm!.DayOfTours!)
                .ThenInclude(d => d.DayActivities!)
                .ThenInclude(da => da.Activity)

                .Include(i => i.Tour)
                .ThenInclude(tm => tm!.DayOfTours!)
                .ThenInclude(i => i.DayActivities!)
                .ThenInclude(i => i.LocationActivity)
                .ThenInclude(i => i!.Place)
                .ThenInclude(i => i!.Location)
                    .OrderBy(s => s.OpenDate).AsNoTracking();

            var scheduleDTOs = await query.Select(i => i.Map()).ToArrayAsync();


            return Ok(new RestDTO<ScheduleDTO[]?>()
            {
                Data = scheduleDTOs
            });
        }

        [HttpGet("schedules-user")]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> getSchedulesUser()
        {

            var now = DateTime.UtcNow.AddHours(7);

            var query = _context.Schedules
            .Include(i => i.Bookings)

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

            .Where(s => s.OpenDate <= now && s.StartDate > now);

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

                var tour = _context.Tours.Where(i => i.Id == newScheduleDTO.TourId).First();
                var words = tour.Title.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                var prefix = "";
                switch (words.Count())
                {
                    case 0:
                        prefix = "UK";
                        break;
                    case 1:
                        prefix = String.Concat(words[0].ElementAt(0), words[0].ElementAt(0));
                        break;
                    default:
                        prefix = String.Concat(words[0].ElementAt(0), words[1].ElementAt(0));
                        break;
                }
                var code = String.Concat(prefix, schedule.Id.ToString());
                schedule.Code = code;
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


        [HttpGet("completed/{userId}")]
        public async Task<IActionResult> getScheduleCompletedBy(int userId)
        {
            try
            {
                var schedules = await _context.Bookings
                    .Where(i => i.UserId == userId)
                    .Where(i => i.UserCompletedSchedule != null)
                    .Include(i => i.Schedule)!
                    .ThenInclude(i => i!.Tour)

                    .Include(s => s.Schedule)
                    .ThenInclude(t => t!.Tour)
                    .ThenInclude(tm => tm!.DayOfTours!)
                    .ThenInclude(i => i.DayActivities!)
                    .ThenInclude(i => i.LocationActivity)
                    .ThenInclude(i => i!.Place)
                    .ThenInclude(i => i!.Location)

                    .Include(s => s.Schedule)
                    .ThenInclude(t => t!.Tour)
                    .ThenInclude(t => t!.TourImages)
                    .AsNoTracking()
                    .Select(i => i.Schedule).ToListAsync();

                return Ok(new RestDTO<List<ScheduleDTO>>()
                {
                    Data = schedules.Select(i => i!.Map()).ToList()
                });
            }
            catch (Exception ex)
            {
                return Problem("while getting schedule: " + ex.Message);
            }
        }

        [HttpGet("Staff")]
        public async Task<IActionResult> getSchedulesOfGuide(
            [Required] int staffId,
            String? filter = null,
            int? provinceId = null,
            int? placeId = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            int? stars = null)
        {
            try
            {
                //staff id
                var query = _context.Schedules.AsQueryable()
                    .Where(i => i.Guides!.Any(i => i.StaffId == staffId))
                    .Where(i => i.EndDate > DateTime.UtcNow.AddHours(7));

                //search string
                var searchStr = filter?.Trim();
                if (!String.IsNullOrEmpty(searchStr))
                {
                    query = query.Where(i => i.Tour!.Title.Contains(searchStr));
                }

                //start date, end date
                if (startDate != null && endDate != null)
                {
                    query = query
                        .Where(s => s.StartDate >= startDate && s.EndDate <= endDate);
                }
                else if (startDate != null)
                {
                    query = query
                        .Where(s => s.StartDate >= startDate);
                }
                else if (endDate != null)
                {
                    query = query
                        .Where(s => s.EndDate <= endDate);
                }

                //province
                if (provinceId != null)
                {
                    query = query
                        .Where(i => i.Tour!.DayOfTours!
                            .SelectMany(dayOfTour => dayOfTour.DayActivities!)
                            .Select(i => i.LocationActivity)
                            .Select(i => i!.Place)
                            .Select(i => i!.Location)
                            .Any(i => i!.Id == provinceId));
                }

                //Place Id
                if (placeId != null)
                {
                    query = query
                        .Where(i => i.Tour!.DayOfTours!
                            .SelectMany(dayOfTour => dayOfTour.DayActivities!)
                            .Select(i => i.LocationActivity)
                            .Select(i => i!.Place)
                            .Any(i => i!.Id == placeId));
                }

                //stars
                if (stars != null)
                {
                    query = query
                        .Where(i =>
                            Math.Floor(i.Reviews!
                            .Select(i => i.Rating)
                            .Average()) == stars);
                }

                var schedules = await query
                    .Include(i => i.Bookings)
                    .Include(i => i.Reviews)

                    .Include(i => i!.Tour)

                    .Include(t => t!.Tour)
                    .ThenInclude(tm => tm!.DayOfTours!)
                    .ThenInclude(i => i.DayActivities!)
                    .ThenInclude(i => i.LocationActivity)
                    .ThenInclude(i => i!.Place)
                    .ThenInclude(i => i!.Location)

                    .Include(t => t!.Tour)
                    .ThenInclude(t => t!.TourImages)
                    .AsNoTracking()
                    .ToListAsync();

                return Ok(new RestDTO<List<ScheduleDTOOfAdmin>>()
                {
                    Data = schedules.Select(i => i!.MapToScheduleOfAdmin()).ToList()
                });
            }
            catch (Exception ex)
            {
                return Problem("while getting schedule: " + ex.Message);
            }
        }
    }
}