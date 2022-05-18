using MoviesItAcademy.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MoviesItAcademy.Service.Abstractions
{
    public interface IMovieService
    {
        Task<List<MovieServiceModel>> GetAllMoviesAsync();
        Task<List<MovieServiceModel>> GetAllMoviesWithUploadersAsync();
        Task<List<MovieServiceModel>> GetAllUpcomingMoviesAsync();
        Task<List<MovieServiceModel>> GetAllDeletedMoviesAsync();
        Task<MovieServiceModel> GetMovieByIdAsync(int id);
        Task<MovieServiceModel> GetReleasedMovieByIdAsync(int id);
        Task<MovieServiceModel> GetMovieByIdWithUploaderAsync(int id);
        Task<int> CreateAMovieAsync(MovieServiceModel movieServiceModel);
        Task UpdateAMovieAsync(MovieServiceModel movieServiceModel);
        Task DeleteAMovieAsync(MovieServiceModel movieServiceModel);
        Task SoftDeleteAMovieAsync(MovieServiceModel movieServiceModel);
        Task SoftDeleteExpiredMoviesAsync();
        Task PublishAMovieAsync(int id);
        Task UnpublishAMovieAsync(int id);
    }
}
