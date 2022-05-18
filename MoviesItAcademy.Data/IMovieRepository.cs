using MoviesItAcademy.Domain.Pocos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MoviesItAcademy.Data
{
    public interface IMovieRepository
    {
        Task<List<Movie>> GetAllAsync();
        Task<List<Movie>> GetAllExpiredMoviesAsync();
        Task<List<Movie>> GetAllWithUploadersAsync();
        Task<List<Movie>> GetAllUpcomingAsync();
        Task<List<Movie>> GetAllDeletedAsync();
        Task<Movie> GetAsync(int id);
        Task<Movie> GetWithUploaderAsync(int id);
        Task<Movie> GetNoTrackingAsync(int id);
        Task<int> CreateAsync(Movie movie);
        Task UpdateAsync(Movie movie);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
