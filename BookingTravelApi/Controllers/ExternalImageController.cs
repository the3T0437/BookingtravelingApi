using BookingTravelApi.Domains;
using BookingTravelApi.DTO;
using BookingTravelApi.DTO.booking;
using BookingTravelApi.DTO.ExternalImage;
using BookingTravelApi.Extensions;
using BookingTravelApi.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingTravelApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ExternalImageController : Controller
    {
        private ApplicationDbContext _context;
        private readonly ILogger<BookingController> _logger;
        public ExternalImageController(ILogger<BookingController> logger, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateImage(CreateExternalImageDTO createExternalImage)
        {
            ExternalImage? image;
            try
            {
                image = await createExternalImage.Map();
                await _context.ExternalImages.AddAsync(image);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return BadRequest(new ErrorDTO(e.Message));
            }

            return Ok(new RestDTO<ExternalImageDTO>()
            {
                Data = image.Map()
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var image = await _context.ExternalImages.FindAsync(id);
            if (image == null)
            {
                return NotFound("Image can't be found");
            }

            ImageInfrastructure.DeleteImage(image.Path);

            _context.ExternalImages.Remove(image);
            await _context.SaveChangesAsync();
            return Ok(new RestDTO<ExternalImageDTO>()
            {
                Data = image.Map()
            });
        }
    }


}