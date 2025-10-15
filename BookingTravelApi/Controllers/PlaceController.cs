using System.Linq.Dynamic.Core;
using BookingTravelApi.Domains;
using BookingTravelApi.DTO;
using BookingTravelApi.DTO.Location;
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
        public async Task<IActionResult> getPlaces(String orderBy = "Name", String sortBy = "ASC", String? filter = null)
        {
            var query = _context.Places.AsQueryable();

            if (!String.IsNullOrEmpty(filter))
            {
                query = query.Where(place => place.Name.Contains(filter));
            }
            query = query.OrderBy($"{orderBy} {sortBy}").AsQueryable();

            var places = await query.ToArrayAsync();
            var placeDTOs = places.Select(place => place.Map()).ToArray();

            return Ok(new RestDTO<PlaceDTO[]?>()
            {
                Data = placeDTOs
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
