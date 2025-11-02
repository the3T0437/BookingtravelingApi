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
using Microsoft.EntityFrameworkCore.Query;
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
        public async Task<IActionResult> GetTours(
            String SortBy = "Title",
            String SortOrder = "ASC",
            String? filter = null,
            int? provinceId = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            int? stars = null)
        {
            var query = _context.Tours.AsQueryable();

            //search string
            var searchStr = filter?.Trim();
            if (!String.IsNullOrEmpty(searchStr))
            {
                query = query.Where(i => i.Title.Contains(searchStr));
            }

            //start date
            if (startDate != null && endDate != null)
            {
                query = query
                    .Where(t => t.Schedules.Any(s => s.StartDate >= startDate && s.EndDate <= endDate));
            }
            else if (startDate != null)
            {
                query = query
                    .Where(t => t.Schedules.Any(s => s.StartDate >= startDate));
            }
            else if (endDate != null)
            {
                query = query
                    .Where(t => t.Schedules.Any(s => s.EndDate <= endDate));
            }

            //province
            if (provinceId != null)
            {
                query = query
                    .Where(i => i.DayOfTours!
                        .SelectMany(dayOfTour => dayOfTour.DayActivities!)
                        .Select(i => i.LocationActivity)
                        .Select(i => i!.Place)
                        .Select(i => i!.Location)
                        .Any(i => i!.Id == provinceId));
            }

            if (stars != null)
            {
                query = query
                    .Where(i => 
                        i.Schedules
                            .SelectMany(s => s.Reviews)
                            .Select(i => i.Rating)
                            .Average() > stars);
            }

            query = query.OrderBy($"{SortBy} {SortOrder}");

            var tours = await query.Include(t => t.TourImages!)
                .Include(ti => ti.DayOfTours!)
                .ThenInclude(d => d.DayActivities!)
                .ThenInclude(da => da.Activity!)

                .Include(ti => ti.DayOfTours!)
                .ThenInclude(d => d.DayActivities!)
                .ThenInclude(da => da.LocationActivity!)
                .ThenInclude(lo => lo.Place!)
                .ThenInclude(p => p.Location)

                .Include(i => i.DayOfTours!)
                !.ThenInclude(i => i.DayActivities!)
                !.ThenInclude(i => i.LocationActivity)
                !.ThenInclude(i => i.ActivityAndLocations)
                !.ThenInclude(i => i.Activity)

                .ToListAsync();

            var tourDTOs = await query.Select(i => i.Map()).ToArrayAsync();

            return Ok(new RestDTO<TourDTO[]>()
            {
                Data = tourDTOs
            });
        }

        [HttpGet("getMostFavoriteTour")]
        public async Task<IActionResult> getMostFavoriteTour()
        {
            var query = _context.Tours.AsQueryable();
            query = query.Include(i => i.Favorites).OrderBy(i => i.Favorites!.Count).Reverse();

            var tours = await query.Include(t => t.TourImages!)
                .Include(ti => ti.DayOfTours!)
                .ThenInclude(d => d.DayActivities!)
                .ThenInclude(da => da.Activity!)

                .Include(ti => ti.DayOfTours!)
                .ThenInclude(d => d.DayActivities!)
                .ThenInclude(da => da.LocationActivity!)
                .ThenInclude(lo => lo.Place!)
                .ThenInclude(p => p.Location)

                .Include(i => i.DayOfTours!)
                !.ThenInclude(i => i.DayActivities!)
                !.ThenInclude(i => i.LocationActivity)
                !.ThenInclude(i => i.ActivityAndLocations)
                !.ThenInclude(i => i.Activity)
                .Take(3)
                .ToListAsync();
            var tourDTOs = tours.Select(i => i.Map()).ToArray();

            return Ok(new RestDTO<TourDTO[]>()
            {
                Data = tourDTOs
            });
        }

        [HttpGet("getMostRecent")]
        public async Task<IActionResult> getMostRecentTour()
        {
            var query = _context.Tours.AsQueryable();
            query = query
                .Where(t => t.Schedules.Any(s => s.OpenDate > DateTime.Now))
                .OrderBy(t => t.Schedules
                    .Where(s => s.OpenDate > DateTime.Now)
                    .Select(s => (DateTime?)s.StartDate)
                    .Min() ?? DateTime.MaxValue);

            var tempTours = query.ToList();

            var tours = await query.Include(t => t.TourImages!)
                .Include(ti => ti.DayOfTours!)
                .ThenInclude(d => d.DayActivities!)
                .ThenInclude(da => da.Activity!)

                .Include(ti => ti.DayOfTours!)
                .ThenInclude(d => d.DayActivities!)
                .ThenInclude(da => da.LocationActivity!)
                .ThenInclude(lo => lo.Place!)
                .ThenInclude(p => p.Location)

                .Include(i => i.DayOfTours!)
                !.ThenInclude(i => i.DayActivities!)
                !.ThenInclude(i => i.LocationActivity)
                !.ThenInclude(i => i.ActivityAndLocations)
                !.ThenInclude(i => i.Activity)
                .Take(3)
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
            var tour = await _context.Tours.Include(i => i.TourImages)!
                .Include(i => i.DayOfTours!)
                .ThenInclude(i => i.DayActivities!)
                .ThenInclude(i => i.Activity)

                .Include(i => i.DayOfTours!)
                .ThenInclude(i => i.DayActivities!)
                .ThenInclude(i => i.LocationActivity!)
                .ThenInclude(i => i.Place!)
                .ThenInclude(i => i.Location)
                .Where(i => i.Id == id)

                .Include(i => i.DayOfTours!)
                !.ThenInclude(i => i.DayActivities!)
                !.ThenInclude(i => i.LocationActivity)
                !.ThenInclude(i => i.ActivityAndLocations)
                !.ThenInclude(i => i.Activity)
                .FirstOrDefaultAsync();
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
            List<String> tempImages = [];
            try
            {
                var newTour = await newTourDTO.Map();
                tempImages = newTour.TourImages?.Select(i => i.Path).ToList() ?? [];
                await _context.Tours.AddAsync(newTour);
                await _context.SaveChangesAsync();

                return await GetTour(newTour.Id);
            }
            catch (Exception)
            {
                tempImages.ForEach(path => ImageInfrastructure.DeleteImage(path));
                return BadRequest(new ErrorDTO("request is not valid"));
            }
        }

        [HttpPut(Name = "UpdateTour")]
        public async Task<IActionResult> UpdateTour(UpdateTourDTO updateTourDTO)
        {
            List<String> newImagePaths = [];
            try
            {
                var tour = await _context.Tours
                    .Include(i => i.TourImages)
                    .Include(i => i.DayOfTours)
                    .Where(i => i.Id == updateTourDTO.Id)
                    .FirstOrDefaultAsync();
                if (tour == null)
                {
                    return NotFound(new ErrorDTO("Tour not found"));
                }

                if (!String.IsNullOrEmpty(updateTourDTO.Title))
                {
                    tour.Title = updateTourDTO.Title;
                }

                if (updateTourDTO.Price != null)
                {
                    tour.Price = updateTourDTO.Price.Value;
                }
                if (updateTourDTO.PercentDeposit != null)
                {
                    tour.PercentDeposit = updateTourDTO.PercentDeposit.Value;
                }

                if (!String.IsNullOrEmpty(updateTourDTO.Description))
                {
                    tour.Description = updateTourDTO.Description;
                }

                if (updateTourDTO.DayOfTours != null)
                {
                    var dayOfTours = updateTourDTO.DayOfTours.Select(i => i.Map()).ToList();
                    tour.DayOfTours = dayOfTours;
                }

                List<String> retainImages = [];
                if (updateTourDTO.RetainImages != null)
                {
                    retainImages = tour.TourImages?
                        .Where((image) => updateTourDTO.RetainImages.Any((url) => url.Contains(image.Path)))
                        .Select((i) => i.Path).ToList() ?? [];
                }

                List<String> oldImages = [];
                if (updateTourDTO.TourImages != null)
                {
                    oldImages = tour.TourImages!.Select(i => i.Path)
                        .Where((imagePath) => retainImages.Contains(imagePath) == false)
                        .ToList();
                    newImagePaths = await ImageInfrastructure.WriteImages(updateTourDTO.TourImages);
                    newImagePaths.AddRange(retainImages);
                    var tourImages = newImagePaths.Select(i => new TourImage() { Path = i }).ToList();
                    tour.TourImages = tourImages;
                }

                _context.Tours.Update(tour);
                await _context.SaveChangesAsync();
                oldImages.ForEach(path => ImageInfrastructure.DeleteImage(path));

                return await GetTour(updateTourDTO.Id);
            }
            catch (Exception)
            {
                newImagePaths.ForEach(path => ImageInfrastructure.DeleteImage(path));
                return BadRequest(new ErrorDTO("request is not valid"));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTour(int id)
        {
            var tour = await _context.Tours.Include(t => t.TourImages)
                .Include(tm => tm.DayOfTours!)
                !.ThenInclude(d => d.DayActivities!)
                !.ThenInclude(da => da.Activity)

                .Include(i => i.DayOfTours!)
                !.ThenInclude(i => i.DayActivities!)
                !.ThenInclude(i => i.LocationActivity)
                !.ThenInclude(i => i!.Place)
                !.ThenInclude(i => i!.Location)

                .Include(i => i.DayOfTours!)
                !.ThenInclude(i => i.DayActivities!)
                !.ThenInclude(i => i.LocationActivity)
                !.ThenInclude(i => i.ActivityAndLocations)
                !.ThenInclude(i => i.Activity)

                .Where(i => i.Id == id).FirstOrDefaultAsync();

            if (tour == null)
            {
                return NotFound(new ErrorDTO("tour not found"));
            }
            _context.Tours.Remove(tour);
            await _context.SaveChangesAsync();

            var tourImages = tour.TourImages ?? [];
            var paths = tourImages.Select(i => i.Path).ToList();

            ImageInfrastructure.DeleteImages(paths);

            return Ok(new RestDTO<TourDTO>()
            {
                Data = tour.Map()
            });
        }
    }
}