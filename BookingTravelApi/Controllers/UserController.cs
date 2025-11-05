using System.Linq.Dynamic.Core;
using BookingTravelApi.Domains;
using BookingTravelApi.DTO;
using BookingTravelApi.DTO.ChangePassword;
using BookingTravelApi.DTO.loginDTO;
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

        [HttpPost("Login")]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> Login(Login login)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == login.email && u.Password == login.password);

            if (user == null)
            {
                return NotFound($"user not found");
            }

            return Ok(new RestDTO<UserDTO>()
            {
                Data = user.Map()
            });
        }

        [HttpPost("loginbyemail")]
        public async Task<IActionResult> LoginByEmail(Login login)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == login.email);

            if (user == null)
            {
                return NotFound($"email {login.email} not found");
            }

            return Ok(new RestDTO<UserDTO>
            {
                Data = user.Map()
            });

        }

        [HttpPatch("update-password/{id}")]
        public async Task<IActionResult> UpdatePassword(int id, [FromBody] ChangePassword changePassword)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return Problem("id not found");
            }

            if (user.Password != changePassword.oldPassword)
            {
                return Ok(new RestDTO<bool>()
                {
                    Data = false
                });
            }


            user.Password = changePassword.newPassword;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Ok(new RestDTO<bool>()
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

                return Ok(new RestDTO<int>()
                {
                    Data = user.Id
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

                return Ok(new RestDTO<Boolean>()
                {
                    Data = true
                });
            }
            catch (Exception ex)
            {
                return Problem("Error updating user: " + ex.Message);
            }
        }

    }
}