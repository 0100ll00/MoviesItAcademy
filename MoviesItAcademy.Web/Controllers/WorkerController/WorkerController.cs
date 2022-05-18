using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MoviesItAcademy.Service.Abstractions;
using System.Threading.Tasks;

namespace MoviesItAcademy.Web.Controllers.WorkerController
{
    [ApiController]
    [Route("[controller]")]
    public class WorkerController : ControllerBase
    {
        private readonly ILogger<WorkerController> _logger;
        private IMovieService _movieService;
        private IBookingService _bookingService;

        public WorkerController(ILogger<WorkerController> logger,
                                IMovieService movieService,     
                                IBookingService bookingService)
        {
            _logger = logger;
            _movieService = movieService;
            _bookingService = bookingService;
        }

        [HttpPost("moviedelition")]
        public async Task<IActionResult> MovieDelition()
        {
            await _movieService.SoftDeleteExpiredMoviesAsync();
            _logger.LogInformation($"Expired movies have been deleted.");
            return Ok();
        }

        [HttpPost("bookingdelition")]
        public async Task<IActionResult> BookingDelition()
        {
            await _bookingService.DeleteExpiredBookingsAsync();
            _logger.LogInformation("Expired bookings have been deleted.");
            return Ok();
        }
    }
}
