using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MoviesItAcademy.Api.Models.Dtos;
using MoviesItAcademy.Service.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoviesItAcademy.Api.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v1/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly IApplicationUserService _applicationUserService;
        private readonly ILogger<BookingController> _logger;

        public BookingController(IBookingService bookingService,
                                 IApplicationUserService applicationUserService,
                                 ILogger<BookingController> logger)
        {
            _bookingService = bookingService;
            _applicationUserService = applicationUserService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetBookings()
        {
            var usedId = (await _applicationUserService.GetUserAsync(User.Identity.Name)).Id;
            var userBookings = await _bookingService.GetUsersBookingsAsync(usedId);
            return Ok(userBookings.Adapt<List<BookingDto>>());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBooking(int id)
        {
            var usedId = (await _applicationUserService.GetUserAsync(User.Identity.Name)).Id;
            var userBooking = await _bookingService.GetUserBookingAsync(id, usedId);
            return Ok(userBooking.Adapt<BookingDto>());
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> BookAMovie(int id)
        {
            var usedId = (await _applicationUserService.GetUserAsync(User.Identity.Name)).Id;
            await _bookingService.BookAMovieAsync(id, usedId, DateTime.Now);
            _logger.LogInformation($"User ({User.Identity.Name}) has booked a ticket for movie Id: {id}.");
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> BuyATicket(int id)
        {
            var usedId = (await _applicationUserService.GetUserAsync(User.Identity.Name)).Id;
            await _bookingService.BuyATicketAsync(id, usedId);
            _logger.LogInformation($"User ({User.Identity.Name}) has purchased a ticket for movie Id: {id}.");
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> UnbookAMovie(int id)
        {
            var usedId = (await _applicationUserService.GetUserAsync(User.Identity.Name)).Id;
            await _bookingService.UnbookAMovieAsync(id, usedId);
            _logger.LogInformation($"User ({User.Identity.Name}) has refunded/unbooked a ticket for movie Id: {id}.");
            return Ok();
        }
    }
}
