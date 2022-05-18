using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoviesItAcademy.Service.Abstractions;
using MoviesItAcademy.Identity.Models.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace MoviesItAcademy.Identity.Controllers
{
    [Authorize(Roles = "Administrator, Moderator")]
    public class BookingController : Controller
    {
        private readonly ILogger<BookingController> _logger;
        private readonly IBookingService _bookingService;

        public BookingController(ILogger<BookingController> logger, IBookingService bookingService)
        {
            _logger = logger;
            _bookingService = bookingService;
        }

        public async Task<IActionResult> UserBookings(int id)
        {
            _logger.LogInformation($"User ({User.Identity.Name}) has accessed bookings of user Id: {id}.");
            var bookings = await _bookingService.GetUsersBookingsAsync(id);
            var bookingsDto = bookings.Adapt<List<BookingDto>>();
            return View(bookingsDto);
        }

        public async Task<IActionResult> UnbookAMovie(int movieId, int userId)
        {
            await _bookingService.UnbookAMovieAsync(movieId, userId);
            _logger.LogInformation($"User ({User.Identity.Name}) has unbooked a ticket for movie Id: {movieId}, user Id: {userId}.");
            var id = userId;
            return RedirectToAction("UserBookings", new { id });
        }
    }
}
