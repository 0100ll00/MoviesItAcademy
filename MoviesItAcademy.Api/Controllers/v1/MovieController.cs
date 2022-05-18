using Mapster;
using Microsoft.AspNetCore.Mvc;
using MoviesItAcademy.Api.Models.Dtos;
using MoviesItAcademy.Service.Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoviesItAcademy.Api.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v1/[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMovies()
        {
            var moviesServiceModel = await _movieService.GetAllMoviesAsync();
            var moviesDto = moviesServiceModel.Adapt<List<MovieDto>>();
            return Ok(moviesDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovie(int id)
        {
            var moviesServiceModel = await _movieService.GetReleasedMovieByIdAsync(id);
            var movieDto = moviesServiceModel.Adapt<MovieDto>();
            return Ok(movieDto);
        }
    }
}
