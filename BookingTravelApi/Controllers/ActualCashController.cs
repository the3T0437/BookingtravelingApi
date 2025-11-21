using System.Reflection.Metadata.Ecma335;
using BookingTravelApi.Domains;
using BookingTravelApi.DTO;
using BookingTravelApi.DTO.actualCashDTO;
using BookingTravelApi.DTO.actualYearDTO;
using BookingTravelApi.DTO.createActualCashDTO;
using BookingTravelApi.DTO.updateActualCashDTO;
using BookingTravelApi.Extensions;
using BookingTravelApi.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace BookingTravelApi.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class ActualCashController : Controller
    {
        private ApplicationDbContext _context;
        private readonly ILogger<ActualCashController> _logger;

        public ActualCashController(ILogger<ActualCashController> logger, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("actualcash-month")]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> GetActualCashMonth()
        {
            var actualCashs = await _context.Actualcashs.AsNoTracking().ToArrayAsync();

            if (actualCashs == null)
            {
                return Problem("Danh sách trống");
            }

            var timeNow = DateTimeHelper.GetVietNamTime();

            var resultActualCashs = actualCashs.Where(a => a.CreatedAt.Year == timeNow.Year).ToList();

            return Ok(new RestDTO<ActualCashMonthDTO>
            {
                Data = resultActualCashs.MapMonth()
            });
        }

        [HttpGet("actualcash-year")]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> GetActualCashYear()
        {
            try
            {
                var actualCashs = await _context.Actualcashs.AsNoTracking().ToArrayAsync();

                if (actualCashs == null)
                {
                    return Problem("Danh sách trống");
                }

                int maxYear = actualCashs.Max(a => a.CreatedAt.Year);
                int startYear = maxYear - 4;

                var resultActualCashs = actualCashs.Where(a => a.CreatedAt.Year >= startYear && a.CreatedAt.Year <= maxYear).ToList();

                return Ok(new RestDTO<List<ActualCashYearDTO>>
                {
                    Data = resultActualCashs.MapYear()
                });
            }
            catch (Exception e)
            {
                return Problem($"Lỗi {e.Message}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateActualCash(UpdateActualCashDTO updateActualCashDTO)
        {
            try
            {
                var actualCash = await _context.Actualcashs.FindAsync(updateActualCashDTO.Id);

                if (actualCash == null)
                {
                    return Problem("Id not found");
                }

                actualCash.money = updateActualCashDTO.money;

                _context.Actualcashs.Update(actualCash);
                await _context.SaveChangesAsync();

                return Ok(new RestDTO<bool>
                {
                    Data = true
                });
            }
            catch (Exception e)
            {
                return Problem($"Lỗi {e.Message}");
            }

        }

        [HttpPost]
        public async Task<IActionResult> CreateActualCash(CreateActualCashDTO createActualCashDTO)
        {
            try
            {
                var actualCash = createActualCashDTO.Map();

                _context.Actualcashs.Add(actualCash);
                await _context.SaveChangesAsync();

                return Ok(new RestDTO<int>
                {
                    Data = actualCash.Id
                });
            }
            catch (Exception e)
            {
                return Problem($"Lỗi {e.Message}");
            }
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteActualCash(int Id)
        {
            try
            {
                var actualCash = await _context.Actualcashs.FindAsync(Id);

                if (actualCash == null)
                {
                    return Problem("Id not found");
                }

                _context.Actualcashs.Remove(actualCash);
                await _context.SaveChangesAsync();

                return Ok(new RestDTO<bool>
                {
                    Data = true
                });
            }
            catch (Exception e)
            {
                return Problem($"Lỗi {e.Message}");
            }
        }
    }
}