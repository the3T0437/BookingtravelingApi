using BookingTravelApi.Domains;
using BookingTravelApi.DTO;
using BookingTravelApi.DTO.role;
using BookingTravelApi.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace BookingTravelApi.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class RoleController : Controller
    {
        private ApplicationDbContext _context;
        private readonly ILogger<RoleController> _logger;
        public RoleController(ILogger<RoleController> logger, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> GetRoles()
        {
            try
            {
                var roles = await _context.Roles.AsNoTracking().ToArrayAsync();

                var result = roles.Select(s => s.Map()).ToList();

                return Ok(new RestDTO<List<RoleDTO>>
                {
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return Problem($"Lỗi {ex.Message}");
            }
        }

        [HttpGet("{Id}")]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> GetRoleById(int Id)
        {
            try
            {
                var role = await _context.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.Id == Id);

                if (role == null)
                {
                    return Problem("Id not found");
                }

                var result = role.Map();

                return Ok(new RestDTO<RoleDTO>
                {
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return Problem($"Lỗi {ex.Message}");
            }
        }

    }
}