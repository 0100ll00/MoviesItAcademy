using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MoviesItAcademy.Identity.Models.Dtos;
using MoviesItAcademy.Identity.Models.RequestModels;
using MoviesItAcademy.Service.Abstractions;
using MoviesItAcademy.Service.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using X.PagedList;

namespace MoviesItAcademy.Identity.Controllers
{
    public class MovieController : Controller
    {
        private readonly ILogger<MovieController> _logger;
        private readonly IMovieService _movieService;

        public MovieController(ILogger<MovieController> logger, IMovieService movieService)
        {
            _logger = logger;
            _movieService = movieService;
        }

        [HttpGet]
        public async Task<IActionResult> MovieList(int? p)
        {
            int pizeSize = 5;
            int pageIndex = p.HasValue ? Convert.ToInt32(p) : 1;

            //var movieList = await _movieService.GetAllMoviesAsync();
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
            var movie = await _movieService.GetMovieByIdAsync(id);
            var movieDto = movie.Adapt<MovieDto>();
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
            var movieServiceModel = _movieService.GetMovieByIdAsync(id).Result;
            var movieRequestModel = movieServiceModel.Adapt<MovieRequestModel>();

            if (movieRequestModel == null)
                return NotFound();

            return View(movieRequestModel);
        }
        [Authorize(Roles = "Moderator, Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAMovie(MovieRequestModel movieRequestModel)
        {
            var movieServiceModel = movieRequestModel.Adapt<MovieServiceModel>();
            movieServiceModel.Uploader = (await _movieService.GetMovieByIdWithUploaderAsync(movieServiceModel.Id)).Uploader;

            _logger.LogInformation($"User ({User.Identity.Name}) has modified ({movieRequestModel.Title}).");
            await _movieService.UpdateAMovieAsync(movieServiceModel);
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
        public async Task<IActionResult> DeleteAMovie(MovieRequestModel movieRequestModel)
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
        public async Task<IActionResult> DeleteAMovieSoft(MovieRequestModel movieRequestModel)
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
