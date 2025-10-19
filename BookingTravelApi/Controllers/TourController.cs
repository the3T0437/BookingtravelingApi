using System.Linq.Dynamic.Core;
using System.Reflection;
using BookingTravelApi.Domains;
using BookingTravelApi.DTO;
using BookingTravelApi.DTO.DayActivity;
using BookingTravelApi.DTO.DayOfTour;
using BookingTravelApi.DTO.Tour;
using BookingTravelApi.Extensions;
using BookingTravelApi.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Filters;

namespace BookingTravelApi.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class TourController : Controller
    {
        private ApplicationDbContext _context;
        private ILogger<TourController> _logger;

        public TourController(ApplicationDbContext context, ILogger<TourController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet(Name = "getTours")]
        public async Task<IActionResult> GetTours(String SortBy = "Title", String SortOrder = "ASC", String? filter = null)
        {
            var query = _context.Tours.AsQueryable();

            var searchStr = filter?.Trim();
            if (!String.IsNullOrEmpty(searchStr))
            {
                query = query.Where(i => i.Title.Contains(searchStr));
            }
            query = query.OrderBy($"{SortBy} {SortOrder}");

            var tours = await query.Include(i => i.TourImages)
                .Include(i => i.DayOfTours)
                .ThenInclude(i => i.DayActivities)
                .ThenInclude(i => i.Activity)
                .Include(i => i.DayOfTours)
                .ThenInclude(i => i.DayActivities)
                .ThenInclude(i => i.LocationActivity)
                .ThenInclude(i => i.Place)
                .ThenInclude(i => i.Location)
                .ToListAsync();
            var tourDTOs = tours.Select(i => i.Map()).ToArray();

            return Ok(new RestDTO<TourDTO[]>()
            {
                Data = tourDTOs
            });
        }


        [HttpGet("{id:int}", Name = "getTour")]
        public async Task<IActionResult> GetTour(int id)
        {
            var tour = await _context.Tours.Include(i => i.TourImages).Include(i => i.DayOfTours).ThenInclude(i => i.DayActivities).ThenInclude(i => new { i.Activity, i.LocationActivity }).Where(i => i.Id == id).FirstOrDefaultAsync();
            if (tour == null)
            {
                return NotFound(new
                {
                    message = "Tour not found"
                });
            }

            return Ok(new RestDTO<TourDTO>()
            {
                Data = tour.Map()
            });
        }

        [HttpPost(Name = "CreateTour")]
        public async Task<IActionResult> CreateTour(CreateTourDTO newTourDTO)
        {
            var newTour = await newTourDTO.Map();
            await _context.Tours.AddAsync(newTour);
            await _context.SaveChangesAsync();

            return Ok(new RestDTO<TourDTO>()
            {
                Data = newTour.Map()
            });
        }
    }
}