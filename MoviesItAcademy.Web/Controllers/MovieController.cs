using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoviesItAcademy.Web.Models.Dtos;
using MoviesItAcademy.Web.Models.RequestModels;
using MoviesItAcademy.Service.Abstractions;
using MoviesItAcademy.Service.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Linq;
using System;
using X.PagedList;
using Microsoft.Extensions.Logging;

namespace MoviesItAcademy.Web.Controllers
{
    public class MovieController : Controller
    {
        private readonly ILogger<MovieController> _logger;
        private readonly IMovieService _movieService;
        private readonly IBookingService _bookingService;
        private readonly IApplicationUserService _applicationUserService;

        public MovieController(ILogger<MovieController> logger,
                               IMovieService movieService,
                               IBookingService bookingService,
                               IApplicationUserService applicationUserService)
        {
            _logger = logger;
            _movieService = movieService;
            _bookingService = bookingService;
            _applicationUserService = applicationUserService;
        }

        [HttpGet]
        public async Task<IActionResult> MovieList(int? p)
        {
            int pizeSize = 5;
            int pageIndex = p.HasValue ? Convert.ToInt32(p) : 1;

            var movieList = await _movieService.GetAllMoviesWithUploadersAsync();
            var movieListDto = movieList.Adapt<List<MovieDto>>();
            return View(movieListDto.ToPagedList(pageIndex, pizeSize));
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public async Task<IActionResult> MovieListWithUploaders(int? p)
        {
            int pizeSize = 5;
            int pageIndex = p.HasValue ? Convert.ToInt32(p) : 1;

            var movieList = await _movieService.GetAllMoviesWithUploadersAsync();
            var movieListDto = movieList.Adapt<List<MovieDto>>();
            return View(movieListDto.ToPagedList(pageIndex, pizeSize));
        }

        [Authorize(Roles = "Administrator, Moderator")]
        [HttpGet]
        public async Task<IActionResult> MovieListUpcoming(int? p)
        {
            int pizeSize = 5;
            int pageIndex = p.HasValue ? Convert.ToInt32(p) : 1;

            var movieListFull = await _movieService.GetAllUpcomingMoviesAsync();
            var movieListFullDto = movieListFull.Adapt<List<MovieDto>>();
            return View(movieListFullDto.ToPagedList(pageIndex, pizeSize));
        }

        [Authorize(Roles = "Administrator, Moderator")]
        [HttpGet]
        public async Task<IActionResult> MovieListDeleted(int? p)
        {
            int pizeSize = 5;
            int pageIndex = p.HasValue ? Convert.ToInt32(p) : 1;

            var movieListFull = await _movieService.GetAllDeletedMoviesAsync();
            var movieListFullDto = movieListFull.Adapt<List<MovieDto>>();
            return View(movieListFullDto.ToPagedList(pageIndex, pizeSize));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> MovieDetails(int id)
        {
            var user = await _applicationUserService.GetUserAsync(User.Identity.Name);

            ViewBag.IsBooked = false;
            ViewBag.IsBought = false;
            ViewBag.HasUserBooked = false;
            ViewBag.IsBookable = await _bookingService.IsMovieBookableAsync(id);

            if (user != null)
            {
                ViewBag.IsBooked = await _bookingService.IsMovieBookedAsync(id, user.Id);
                ViewBag.IsBought = await _bookingService.IsTicketBoughtAsync(id, user.Id);
                ViewBag.HasUserBooked = await _bookingService.IsMovieBookedAsync(id, user.Id);
            }

            var movie = await _movieService.GetMovieByIdAsync(id);
            var movieDto = movie.Adapt<MovieDto>();

            var session = HttpContext.Session.GetString("BookRequestList");
            if (session != null)
            {
                var bookingList = JsonConvert.DeserializeObject<List<BookingDto>>(session);
                if (bookingList.Any(x => x.MovieId == movieDto.Id))
                    ViewBag.IsBooked = true;
            }

            return View(movieDto);
        }

        [Authorize(Roles = "Moderator, Administrator")]
        public IActionResult AddAMovie()
        {
            return View();
        }
        [Authorize(Roles = "Moderator, Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAMovie([FromForm] MovieRequestModel movieRequestModel)
        {
            if (!ModelState.IsValid)
                return View();

            var movieServiceModel = movieRequestModel.Adapt<MovieServiceModel>();
            movieServiceModel.Uploader = User.Identity.Name;
            var movieId = await _movieService.CreateAMovieAsync(movieServiceModel);
            _logger.LogInformation($"User ({User.Identity.Name}) has added a movie ({movieRequestModel.Title}), Id: {movieId}.");
            return RedirectToAction("MovieList");
        }

        [Authorize(Roles = "Moderator, Administrator")]
        public IActionResult UpdateAMovie(int id)
        {
            var movieServiceModel = _movieService.GetMovieByIdAsync(id).GetAwaiter().GetResult();
            var movieRequestModel = movieServiceModel.Adapt<MovieRequestModel>();

            if (movieRequestModel == null)
                return NotFound();

            return View(movieRequestModel);
        }
        [Authorize(Roles = "Moderator, Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAMovie([FromForm] MovieRequestModel movieRequestModel)
        {
            var movieServiceModel = movieRequestModel.Adapt<MovieServiceModel>();
            movieServiceModel.Uploader = (await _movieService.GetMovieByIdWithUploaderAsync(movieServiceModel.Id)).Uploader;

            await _movieService.UpdateAMovieAsync(movieServiceModel);
            _logger.LogInformation($"User ({User.Identity.Name}) has modified ({movieRequestModel.Title}).");
            return RedirectToAction("MovieList");
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult DeleteAMovie(int id)
        {
            var movieServiceModel = _movieService.GetMovieByIdAsync(id).Result;
            var movieRequestModel = movieServiceModel.Adapt<MovieRequestModel>();

            if (movieRequestModel == null)
                return NotFound();

            return View(movieRequestModel);
        }
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAMovie([FromForm] MovieRequestModel movieRequestModel)
        {
            await _movieService.DeleteAMovieAsync(movieRequestModel.Adapt<MovieServiceModel>());
            return RedirectToAction("MovieList");
        }

        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult DeleteAMovieSoft(int id)
        {
            var movieServiceModel = _movieService.GetMovieByIdAsync(id).Result;
            var movieRequestModel = movieServiceModel.Adapt<MovieRequestModel>();

            if (movieRequestModel == null)
                return NotFound();

            return View(movieRequestModel);
        }
        [Authorize(Roles = "Administrator, Moderator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAMovieSoft([FromForm] MovieRequestModel movieRequestModel)
        {
            var movieServiceModel = await _movieService.GetMovieByIdWithUploaderAsync(movieRequestModel.Id);
            await _movieService.SoftDeleteAMovieAsync(movieServiceModel);
            _logger.LogInformation($"User ({User.Identity.Name}) has moved ({movieServiceModel.Title}) to archive.");
            return RedirectToAction("MovieList");
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> PublishAMovie(int id)
        {
            await _movieService.PublishAMovieAsync(id);
            _logger.LogInformation($"User ({User.Identity.Name}) has published a movie with Id: {id}.");
            return RedirectToAction("MovieList");
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UnpublishAMovie(int id)
        {
            await _movieService.UnpublishAMovieAsync(id);
            _logger.LogInformation($"User ({User.Identity.Name}) has unpublished a movie with Id: {id}.");
            return RedirectToAction("MovieListUpcoming");
        }
    }
}
