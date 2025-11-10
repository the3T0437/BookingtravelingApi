using BookingTravelApi.Domains;
using BookingTravelApi.DTO;
using BookingTravelApi.DTO.booking;
using BookingTravelApi.DTO.favorite;
using BookingTravelApi.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingTravelApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FavoriteController : Controller
    {
        private ApplicationDbContext _context;
        private readonly ILogger<BookingController> _logger;
        public FavoriteController(ILogger<BookingController> logger, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("{userId}")]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> getFavorite(int? userId)
        {
            try
            {
                if (userId == null)
                {
                    return Problem("id not found");
                }

                var query = await _context.Favorites
                .Where(f => f.UserId == userId)

                .Include(t => t!.Tour)
                .ThenInclude(tm => tm!.TourImages)

                .Include(t => t!.Tour)
                .ThenInclude(d => d!.DayOfTours)
                !.ThenInclude(d => d.DayActivities!)
                .ThenInclude(da => da.LocationActivity!)
                .ThenInclude(lo => lo.Place!)
                .ThenInclude(p => p.Location)
                .AsNoTracking().ToListAsync();

                var favorite = query.Select(i => i.Map()).ToArray();

                return Ok(new RestDTO<FavoriteDTO[]?>()
                {
                    Data = favorite
                });

            }
            catch (Exception ex)
            {
                return Problem($"Get fail {ex.Message}");
            }
        }

        [HttpPost(Name = "CreateFavorite")]
        public async Task<IActionResult> createFavorite(CreateFavoriteDTO newFavorite)
        {
            try
            {
                var favorite = newFavorite.Map();
                await _context.Favorites.AddAsync(favorite);
                await _context.SaveChangesAsync();

                return Ok(new RestDTO<int>
                {
                    Data = favorite.TourId
                });
            }
            catch (Exception ex)
            {
                return Problem($"Create fail {ex.Message}");
            }
        }

        [HttpDelete(Name = "DeleteFavorite")]
        public async Task<IActionResult> deleteFavorite(int tourId, int userId)
        {
            try
            {
                var favorite = await _context.Favorites.Where(t => t.TourId == tourId && t.UserId == userId).FirstOrDefaultAsync();

                if (favorite == null)
                {
                    return NotFound($"Favorite with Id {tourId} or {userId} not found.");
                }
                _context.Favorites.Remove(favorite);
                await _context.SaveChangesAsync();

                return Ok(new RestDTO<Boolean>()
                {
                    Data = true
                });
            }
            catch (Exception ex)
            {
                return Problem($"Delete favorite fail {ex.Message}");
            }
        }

    }
}