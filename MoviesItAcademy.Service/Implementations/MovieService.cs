using Mapster;
using MoviesItAcademy.Data;
using MoviesItAcademy.Domain.Pocos;
using MoviesItAcademy.Service.Abstractions;
using MoviesItAcademy.Service.Exceptions;
using MoviesItAcademy.Service.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoviesItAcademy.Service.Implementations
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly IBookingRepository _bookingRepository;

        public MovieService(IMovieRepository movieRepository,
                            IApplicationUserRepository applicationUserRepository,
                            IBookingRepository bookingRepository)
        {
            _movieRepository = movieRepository;
            _applicationUserRepository = applicationUserRepository;
            _bookingRepository = bookingRepository;
        }

        public async Task<int> CreateAMovieAsync(MovieServiceModel movieServiceModel)
        {
            if (await _movieRepository.ExistsAsync(movieServiceModel.Id))
                throw new MovieIdAlreadyExistsException(movieServiceModel.Id);

            var adaptedMovie = movieServiceModel.Adapt<Movie>();
            adaptedMovie.UploadedById = (await _applicationUserRepository.GetNoTrackingAsync(movieServiceModel.Uploader)).Id;
            var insertedMovieId = await _movieRepository.CreateAsync(adaptedMovie);
            return insertedMovieId;
        }
        
        public async Task DeleteAMovieAsync(MovieServiceModel movieServiceModel)
        {
            if (!await _movieRepository.ExistsAsync(movieServiceModel.Id))
                throw new MovieIdNotFoundException(movieServiceModel.Id);

            await _movieRepository.DeleteAsync(movieServiceModel.Id);
        }
        
        public async Task SoftDeleteAMovieAsync(MovieServiceModel movieServiceModel)
        {
            if (!await _movieRepository.ExistsAsync(movieServiceModel.Id))
                throw new MovieIdNotFoundException(movieServiceModel.Id);

            var adaptedMovie = movieServiceModel.Adapt<Movie>();
            adaptedMovie.UploadedById = (await _applicationUserRepository.GetNoTrackingAsync(movieServiceModel.Uploader)).Id;
            adaptedMovie.IsActive = false;
            adaptedMovie.IsDeleted = true;

            await _movieRepository.UpdateAsync(adaptedMovie);
        }
        
        public async Task<List<MovieServiceModel>> GetAllMoviesWithUploadersAsync()
        {
            var result = await _movieRepository.GetAllWithUploadersAsync();
            var adaptedResult = result.Adapt<List<MovieServiceModel>>();
            return adaptedResult;
        }
        
        public async Task<List<MovieServiceModel>> GetAllMoviesAsync()
        {
            var result = await _movieRepository.GetAllAsync();
            var adaptedResult = result.Adapt<List<MovieServiceModel>>();
            return adaptedResult;
        }
        
        public async Task<MovieServiceModel> GetMovieByIdAsync(int id)
        {
            var movie = await _movieRepository.GetAsync(id);
            if (movie == null)
                throw new MovieIdNotFoundException(id);

            var adaptedMovie = movie.Adapt<MovieServiceModel>();
            return adaptedMovie;
        }
        
        public async Task UpdateAMovieAsync(MovieServiceModel movieServiceModel)
        {
            var movie = await _movieRepository.GetNoTrackingAsync(movieServiceModel.Id);
            if (movie == null)
                throw new MovieIdNotFoundException(movieServiceModel.Id);

            if (movieServiceModel.StartsAt != movie.StartsAt)
            {
                var bookingsToUpdate = await _bookingRepository.GetMoviesBookingsAsync(movie.Id);
                if (bookingsToUpdate != null)
                {
                    foreach (var booking in bookingsToUpdate)
                    {
                        if (booking.IsActive)
                        {
                            booking.StartsAt = movieServiceModel.StartsAt;
                            booking.RunsOutAt = movieServiceModel.StartsAt;
                        }
                        else
                        {
                            booking.StartsAt = movieServiceModel.StartsAt;
                            booking.RunsOutAt = movieServiceModel.StartsAt.AddHours(-1);
                        }
                        await _bookingRepository.UpdateABookingAsync(booking);
                    }
                }
            }

            var adaptedMovie = movieServiceModel.Adapt<Movie>();
            adaptedMovie.UploadedById = (await _applicationUserRepository.GetNoTrackingAsync(movieServiceModel.Uploader)).Id;
            await _movieRepository.UpdateAsync(adaptedMovie);
        }
        
        public async Task<List<MovieServiceModel>> GetAllUpcomingMoviesAsync()
        {
            var result = await _movieRepository.GetAllUpcomingAsync();
            var adaptedResult = result.Adapt<List<MovieServiceModel>>();
            return adaptedResult;
        }
        
        public async Task<List<MovieServiceModel>> GetAllDeletedMoviesAsync()
        {
            var result = await _movieRepository.GetAllDeletedAsync();
            var adaptedResult = result.Adapt<List<MovieServiceModel>>();
            return adaptedResult;
        }
        
        public async Task<MovieServiceModel> GetMovieByIdWithUploaderAsync(int id)
        {
            var movie = await _movieRepository.GetWithUploaderAsync(id);
            if (movie == null)
                throw new MovieIdNotFoundException(id);

            var adaptedMovie = movie.Adapt<MovieServiceModel>();
            return adaptedMovie;
        }
        
        public async Task SoftDeleteExpiredMoviesAsync()
        {
            var movies = await _movieRepository.GetAllExpiredMoviesAsync();
            foreach (var movie in movies)
            {
                movie.IsActive = false;
                movie.IsDeleted = true;

                await _movieRepository.UpdateAsync(movie);
            }
        }
        
        public async Task PublishAMovieAsync(int id)
        {
            var movie = await _movieRepository.GetAsync(id);
            if (movie == null)
                throw new MovieIdNotFoundException(id);

            movie.IsActive = true;
            await _movieRepository.UpdateAsync(movie);
        }
        
        public async Task UnpublishAMovieAsync(int id)
        {
            var movie = await _movieRepository.GetAsync(id);
            if (movie == null)
                throw new MovieIdNotFoundException(id);

            movie.IsActive = false;
            await _movieRepository.UpdateAsync(movie);
        }

        public async Task<MovieServiceModel> GetReleasedMovieByIdAsync(int id)
        {
            var movie = await _movieRepository.GetAsync(id);
            if (movie == null)
                throw new MovieIdNotFoundException(id);
            if (movie.IsDeleted)
                throw new MovieHasBeenDeletedException(movie.Title);
            if (!movie.IsActive)
                throw new MovieHasNotBeenReleasedException(movie.Title);

            var adaptedMovie = movie.Adapt<MovieServiceModel>();
            return adaptedMovie;
        }
    }
}
