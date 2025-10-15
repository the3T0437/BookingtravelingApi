using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookingTravelApi.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class ErrorHandler : ControllerBase
    {
        [HttpGet]
        public IActionResult ErrorHandle()
        {
            return Problem();
        }
    }
}
