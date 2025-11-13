using System.Linq.Dynamic.Core;
using BookingTravelApi.Domains;
using BookingTravelApi.DTO;
using BookingTravelApi.DTO.review;
using BookingTravelApi.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingTravelApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ReviewController : Controller
    {
        private ApplicationDbContext _context;
        private readonly ILogger<ReviewController> _logger;

        public ReviewController(ILogger<ReviewController> logger, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost("getReviews")]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> getReviewUser(GetReviewDTO getReviewDTO)
        {
            try
            {
                var query = await _context.Reviews
                .Where(u => u.Schedule != null && u.Schedule!.TourId == getReviewDTO.TourId)
                .Include(t => t.User)

                .Include(i => i.Helpfuls)

                .Include(s => s.Schedule)
                .ThenInclude(g => g!.Tour)

                .Include(s => s.Schedule)
                .ThenInclude(g => g!.Guides)
                !.ThenInclude(st => st!.Staff)
                .ThenInclude(us => us!.User)
                .AsNoTracking().ToListAsync();

                var reviews = query.Select(i => i.Map()).ToList();


                if (getReviewDTO.UserId != null)
                {
                    var helpfulList = await _context.Helpfuls
                        .Where(i => i.UserId == getReviewDTO.UserId && i.Review.Schedule!.TourId == getReviewDTO.TourId)
                        .ToListAsync();

                    var helpfulReviewIds = helpfulList.Select(i => i.ReviewId).ToList();

                    reviews = reviews.Select( i =>
                    {
                        if (helpfulReviewIds.Contains(i.Id))
                        {
                            i.IsHelpful = true;
                        }

                        return i;
                    }).ToList();
                }

                return Ok(new RestDTO<List<ReviewDTO>>()
                {
                    Data = reviews
                });
            }
            catch (Exception ex)
            {
                return Problem($"ERROR GuidesscheduleId");
            }
        }

        [HttpPost(Name = "CreateReview")]
        public async Task<IActionResult> createReview(CreateReviewDTO newReviewDTO)
        {
            try
            {
                var review = newReviewDTO.Map();

                //tim schedule
                var schedule = await _context.Schedules.Include(s => s.Tour).FirstOrDefaultAsync(s => s.Id == review.ScheduleId);
                schedule!.Tour!.TotalReviews += 1;
                schedule!.Tour!.TotalStars += review.Rating;
                
                await _context.Reviews.AddAsync(review);
                await _context.SaveChangesAsync();


                return Ok(new RestDTO<String>()
                {
                    Data = review.Content
                });
            }
            catch (Exception ex)
            {
                return Problem("Error create");
            }
        }

        [HttpDelete(Name = "DeleteReview")]
        public async Task<IActionResult> deleteReview(int userId, int scheduleId)
        {
            try
            {
                var review = await _context.Reviews.Where(r => r.UserId == userId && r.ScheduleId == scheduleId).FirstOrDefaultAsync();
                if (review == null)
                {
                    return NotFound($"Review with Id {userId} or {scheduleId} not found.");
                }

                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();

                return Ok(new RestDTO<Boolean>()
                {
                    Data = true,
                });

            }
            catch (Exception ex)
            {
                return Problem("Eror delete");
            }
        }

    }
}