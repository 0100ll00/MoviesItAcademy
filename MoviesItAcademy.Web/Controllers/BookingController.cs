using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MoviesItAcademy.Service.Abstractions;
using MoviesItAcademy.Web.Models.Dtos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace MoviesItAcademy.Web.Controllers
{
    [Authorize(Roles = "Administrator, Moderator, User")]
    public class BookingController : Controller
    {
        private readonly ILogger<BookingController> _logger;
        private readonly IBookingService _bookingService;
        private readonly IApplicationUserService _applicationUserService;

        public BookingController(ILogger<BookingController> logger,
                                 IBookingService bookingService,
                                 IApplicationUserService applicationUserService)
        {
            _logger = logger;
            _bookingService = bookingService;
            _applicationUserService = applicationUserService;
        }

        [HttpGet]
        public async Task<IActionResult> UserBookings(int? p)
        {
            int pizeSize = 5;
            int pageIndex = p.HasValue ? Convert.ToInt32(p) : 1;

            var usedId = (await _applicationUserService.GetUserAsync(User.Identity.Name)).Id;
            var bookings = await _bookingService.GetUsersBookingsAsync(usedId);
            var bookingsDto = bookings.Adapt<List<BookingDto>>();
            return View(bookingsDto.ToPagedList(pageIndex, pizeSize));
        }

        [AllowAnonymous]
        public async Task<IActionResult> BookAMovie(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                List<BookingDto> bookingList;
                var session = HttpContext.Session.GetString("BookRequestList");
                if (session == null)
                    bookingList = new List<BookingDto>();
                else
                    bookingList = JsonConvert.DeserializeObject<List<BookingDto>>(session);

                var contains = bookingList.Any(x => x.MovieId == id && x.UserId == -1);
                if (contains)
                    return RedirectToAction("MovieDetails", "Movie", new { id });

                var booking = new BookingDto() { MovieId = id, UserId = -1, BookedAt = DateTime.Now };
                bookingList.Add(booking);
                var bookingListString = JsonConvert.SerializeObject(bookingList);

                HttpContext.Session.SetString("BookRequestList", bookingListString);
            }
            else
            {
                var usedId = (await _applicationUserService.GetUserAsync(User.Identity.Name)).Id;
                await _bookingService.BookAMovieAsync(id, usedId, DateTime.Now);
                _logger.LogInformation($"User ({User.Identity.Name}) has booked a ticket for movie Id: {id}.");
            }
            return RedirectToAction("MovieDetails", "Movie", new { id });
        }

        [AllowAnonymous]
        public async Task<IActionResult> UnbookAMovie(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                List<BookingDto> bookingList;
                var session = HttpContext.Session.GetString("BookRequestList");
                if (session == null)
                    bookingList = new List<BookingDto>();
                else
                    bookingList = JsonConvert.DeserializeObject<List<BookingDto>>(session);

                var booking = bookingList.SingleOrDefault(x => x.MovieId == id && x.UserId == -1);
                if (booking == null)
                    return RedirectToAction("MovieDetails", "Movie", new { id });

                bookingList.Remove(booking);
                if (bookingList.Count == 0)
                    HttpContext.Session.Clear();
                else
                {
                    var bookingListString = JsonConvert.SerializeObject(bookingList);
                    HttpContext.Session.SetString("BookRequestList", bookingListString);
                }
            }
            else
            {
                var usedId = (await _applicationUserService.GetUserAsync(User.Identity.Name)).Id;
                await _bookingService.UnbookAMovieAsync(id, usedId);
                _logger.LogInformation($"User ({User.Identity.Name}) has refunded/unbooked a ticket for movie Id: {id}.");
            }
            return RedirectToAction("MovieDetails", "Movie", new { id });
        }

        public async Task<IActionResult> BuyATicket(int id)
        {
            var usedId = (await _applicationUserService.GetUserAsync(User.Identity.Name)).Id;
            await _bookingService.BuyATicketAsync(id, usedId);
            _logger.LogInformation($"User ({User.Identity.Name}) has purchased a ticket for movie Id: {id}.");
            return RedirectToAction("MovieDetails", "Movie", new { id });
        }
    }
}
