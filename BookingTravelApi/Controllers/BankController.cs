using BookingTravelApi.Domains;
using BookingTravelApi.DTO;
using BookingTravelApi.DTO.bank;
using BookingTravelApi.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingTravelApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BankController : Controller
    {
        private ApplicationDbContext _context;
        private readonly ILogger<BankController> _logger;
        public BankController(ILogger<BankController> logger, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet(Name = "getBank")]
        public async Task<IActionResult> getBank()
        {
            try
            {
                var query = await _context.Banks.AsNoTracking().ToListAsync();

                var bank = query.Select(i => i.Map()).ToArray();
                return Ok(new RestDTO<BankDTO[]?>()
                {
                    Data = bank
                });

            }
            catch (Exception ex)
            {
                return Problem($"Get booking fail {ex.Message}");
            }
        }
    }
}