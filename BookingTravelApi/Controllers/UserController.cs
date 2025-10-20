using System.Linq.Dynamic.Core;
using BookingTravelApi.Domains;
using BookingTravelApi.DTO;
using BookingTravelApi.DTO.user;
using BookingTravelApi.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingTravelApi.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private ApplicationDbContext _context;
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> getUsers()
        {
            var query = _context.Users.AsNoTracking();

            var userDTOs = await query.Select(i => i.Map()).ToArrayAsync();

            return Ok(new RestDTO<UserDTO[]?>()
            {
                Data = userDTOs
            });
        }

        [HttpGet("email")]
        public async Task<IActionResult> GetUserByEmail([FromQuery] string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return Problem($"email {email} not found");
            }

            return Ok(new RestDTO<Boolean>
            {
                Data = true
            });
            
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound($"id {id} not found");
            }

            return Ok(new RestDTO<UserDTO?>()
            {
                Data = user.Map()
            });
        }

        [HttpPost(Name = "CreateUser")]
        public async Task<IActionResult> CreateUser(CreateUserDTO newUserDTO)
        {
            try
            {
                var user = newUserDTO.Map();

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return Ok(new RestDTO<UserDTO?>()
                {
                    Data = user.Map()
                });
            }
            catch (Exception ex)
            {
                return Problem("Error creating user: " + ex.Message);
            }
        }

        [HttpPut(Name = "UpdateUser")]
        public async Task<IActionResult> UpdateUser(UpdateUserDTO updatedUser)
        {
            try
            {
                var user = await _context.Users.FindAsync(updatedUser.Id);

                if (user == null)
                {
                    return NotFound($"id {updatedUser.Id} not found");
                }

                updatedUser.UpdateEntity(user);

                await _context.SaveChangesAsync();

                return Ok(new RestDTO<UserDTO?>()
                {
                    Data = user.Map()
                });
            }
            catch (Exception ex)
            {
                return Problem("Error updating user: " + ex.Message);
            }
        }

    }
}