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
            query = query.OrderBy($"${SortBy} {SortOrder}");

            var tours = await query.ToListAsync();
            var tourDTOs = tours.Select(i => i.Map()).ToArray();

            return Ok(new RestDTO<TourDTO[]>()
            {
                Data = tourDTOs
            });
        }


        [HttpGet("{id:int}", Name = "getTour")]
        public async Task<IActionResult> GetTour(int id)
        {
            var tour = await _context.Tours.Where(i => i.Id == id).FirstOrDefaultAsync();
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
            var check = ValidateCreateTourDTO(newTourDTO);
            if (check != null)
            {
                return NotFound(new
                {
                    message = check
                });
            }

            var tour = await _CreateTour(newTourDTO);
            return Ok(new RestDTO<TourDTO>()
            {
                Data = tour.Map()
            });
        }

        private async Task<Tour> _CreateTour(CreateTourDTO newTourDTO)
        {
            var tour = newTourDTO.Map();
            await _context.Tours.AddAsync(tour);
            await _context.SaveChangesAsync();
            var tourImages = await _CreateTourImages(tour.Id, newTourDTO.TourImages);
            var dayOfTours = await _CreateDayOfTour(tour.Id, newTourDTO.DayOfTours);
            tour.TourImages = tourImages;
            tour.DayOfTours = dayOfTours;

            return tour;
        }

        private async Task<List<DayOfTour>> _CreateDayOfTour(int tourId, List<CreateDayOfTourDTO> createDayOfTourDTOs)
        {
            var dayOfTourTasks = createDayOfTourDTOs.Select(async i =>
            {
                var dayOfTour = i.Map(tourId);
                await _context.DayOfTours.AddAsync(dayOfTour);
                await _context.SaveChangesAsync();
                var dayActivities = await _CreateDayActivities(tourId, i.DayActivities);
                dayOfTour.DayActivities = dayActivities;
                return dayOfTour;
            }).ToList();
            var dayOfTours = (await Task.WhenAll(dayOfTourTasks)).ToList();

            return dayOfTours;
        }

        private async Task<List<DayActivity>> _CreateDayActivities(int tourId, List<CreateDayActivityDTO> createDayActivityDTOs)
        {
            var dayActivities = createDayActivityDTOs.Select(i => i.Map(tourId)).ToList();
            await _context.DayActivities.AddRangeAsync(dayActivities);
            await _context.SaveChangesAsync();
            return dayActivities;
        }

        private async Task<List<TourImage>> _CreateTourImages(int tourId, List<IFormFile> images)
        {
            List<TourImage> tourImages = [];
            foreach (var image in images)
            {
                var imagePath = await ImageInfrastructure.WriteImage(image);
                if (imagePath == null)
                {
                    throw new Exception("can't write image");
                }

                var tourImage = new TourImage()
                {
                    Path = imagePath,
                    TourId = tourId
                };

                tourImages.Add(tourImage);
            }
            await _context.TourImages.AddRangeAsync(tourImages);
            await _context.SaveChangesAsync();

            return tourImages;
        }

        private async Task<String?> ValidateCreateTourDTO(CreateTourDTO createTourDTO)
        {
            foreach (var i in createTourDTO.DayOfTours)
            {
                var check = await ValidateCreateDayOfTourDTO(i);
                if (check != null)
                {
                    return check;
                }
            }

            foreach (var i in createTourDTO.TourImages)
            {
                var check = await ValidateIFromFile(i);
                if (check != null)
                {
                    return check;
                }
            }

            return null;
        }

        private async Task<String?> ValidateIFromFile(IFormFile formFile)
        {
            return null;
        }

        private async Task<String?> ValidateCreateDayOfTourDTO(CreateDayOfTourDTO createDayOfTourDTO)
        {
            foreach (var i in createDayOfTourDTO.DayActivities)
            {
                var check = await ValidateCreateDayActivityDTO(i);
                if (check != null)
                {
                    return check;
                }
            }

            return null;
        }

        private async Task<String?> ValidateCreateDayActivityDTO(CreateDayActivityDTO createDayActivityDTO)
        {
            var activity = await _context.Activities.Where(i => i.Id == createDayActivityDTO.ActivityId).FirstOrDefaultAsync();
            if (activity == null)
                return $"activity with id {createDayActivityDTO.ActivityId} doesn't exist";

            var locationActivity = await _context.LocationActivities.Where(i => i.Id == createDayActivityDTO.LocationActivityId).FirstOrDefaultAsync();
            if (locationActivity == null)
                return $"locationActivity with id {createDayActivityDTO.ActivityId} doesn't exist";

            return null;
        }
    }
}