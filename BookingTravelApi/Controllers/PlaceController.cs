using System.Linq.Dynamic.Core;
using BookingTravelApi.Domains;
using BookingTravelApi.DTO;
using BookingTravelApi.DTO.place;
using BookingTravelApi.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingTravelApi.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class PlaceController : Controller
    {
        private ApplicationDbContext _context;
        private readonly ILogger<LocationController> _logger;

        public PlaceController(ILogger<LocationController> logger, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> GetPlaces(String orderBy = "Name", String sortBy = "ASC", String? filter = null, int? locationId = null)
        {
            var query = _context.Places.AsQueryable();

            var searchStr = filter?.Trim();
            if (!String.IsNullOrEmpty(searchStr))
            {
                query = query.Where(place => place.Name.Contains(searchStr));
            }
            if (locationId != null)
            {
                query = query.Where(place => place.LocationId == locationId.Value);
            }
            query = query.OrderBy($"{orderBy} {sortBy}").AsQueryable();

            var places = await query.Include(i => i.Location).ToArrayAsync();
            var placeDTOs = places.Select(place =>
            {
                var placeDTO = place.Map();
                var locationDTO = place.Location?.Map();
                placeDTO.Location = locationDTO;
                return placeDTO;
            }).ToArray();

            return Ok(new RestDTO<PlaceDTO[]?>()
            {
                Data = placeDTOs
            });
        }

        [HttpGet("{id:int}")]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> GetPlace(int id)
        {
            var place = await _context.Places.Where(i => i.Id == id).Include(i => i.Location).FirstOrDefaultAsync();
            if (place == null)
            {
                return NotFound(new
                {
                    message = "not found place"
                });
            }

            var placeDTO = place.Map();
            var locationDTO = place.Location?.Map();
            placeDTO.Location = locationDTO;

            return Ok(new RestDTO<PlaceDTO>()
            {
                Data = placeDTO
            });
        }

        [HttpPost(Name = "CreatePlace")]
        public async Task<IActionResult> CreatePlace(CreatePlaceDTO newPlaceDTO)
        {
            try
            {
                var place = newPlaceDTO.Map();
                await _context.Places.AddAsync(place);
                await _context.SaveChangesAsync();

                // Trả về HTTP 200
                return Ok(new RestDTO<PlaceDTO?>()
                {
                    Data = place?.Map()
                });
            }
            catch (Exception ex)
            {
                return Problem("Error create");
            }
        }

        [HttpPut(Name = "UpdatePlace")]
        public async Task<IActionResult> updatePlace(UpdatePlaceDTO newPlace)
        {
            try
            {
                var location = await _context.Locations.Where(i => i.Id == newPlace.LocationId).FirstOrDefaultAsync();
                if (location == null)
                {
                    return BadRequest(new
                    {
                        message = "location doesn't exist",
                        data = newPlace.LocationId
                    });
                }

                var place = await _context.Places.Where(place => place.Id == newPlace.Id).FirstOrDefaultAsync();

                if (place == null)
                {
                    return NotFound($"Id {newPlace.Id} not found.");
                }

                if (newPlace.LocationId != null)
                {
                    place.LocationId = newPlace.LocationId.Value;
                }

                if (!String.IsNullOrEmpty(newPlace.Name))
                {
                    place.Name = newPlace.Name;
                }


                await _context.SaveChangesAsync();

                // Trả về HTTP 200
                return Ok(new RestDTO<PlaceDTO?>()
                {
                    Data = place?.Map()
                });

            }
            catch (Exception ex)
            {
                return Problem("Error update");
            }

        }
        [HttpDelete(Name = "DeletePlace")]
        public async Task<IActionResult> deletePlace(int id)
        {
            try
            {

                var place = await _context.Places.Where(p => p.Id == id).FirstOrDefaultAsync();

                if (place == null)
                {
                    // Trả về HTTP 404 Not Found
                    return NotFound($"Place with Id {id} not found.");
                }

                _context.Places.Remove(place);
                await _context.SaveChangesAsync();

                // Trả về HTTP 200
                return Ok(new RestDTO<PlaceDTO?>()
                {
                    Data = place?.Map()
                });
            }
            catch (Exception ex)
            {
                return Problem("Error delete");
            }
        }
    }
}
