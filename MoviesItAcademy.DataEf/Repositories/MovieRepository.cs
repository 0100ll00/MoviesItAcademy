using Microsoft.EntityFrameworkCore;
using MoviesItAcademy.Data;
using MoviesItAcademy.Domain.Pocos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesItAcademy.DataEf.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private IBaseRepository<Movie> _baseRepository;

        public MovieRepository(IBaseRepository<Movie> baseRepository)
        {
            _baseRepository = baseRepository;
        }

        public async Task<int> CreateAsync(Movie movie)
        {
            await _baseRepository.AddAsync(movie);
            return movie.Id;
        }
        
        public async Task DeleteAsync(int id)
        {
            await _baseRepository.RemoveAsync(id);
        }
        
        public async Task<bool> ExistsAsync(int id)
        {
            return await _baseRepository.AnyAsync(x => x.Id == id);
        }
        
        public async Task<List<Movie>> GetAllAsync()
        {
            return (await _baseRepository.GetAllAsync())
                .Where(x => x.IsActive == true && x.IsDeleted == false)
                .ToList();
        }
        
        public async Task<List<Movie>> GetAllWithUploadersAsync()
        {
            return await _baseRepository.Table
                .Where(x => x.IsActive == true && x.IsDeleted == false)
                .Include(x => x.UploadedBy)
                .ToListAsync();
        }
        
        public async Task<Movie> GetAsync(int id)
        {
            return await _baseRepository.GetAsync(id);
        }
        
        public async Task<Movie> GetNoTrackingAsync(int id)
        {
            return await _baseRepository.TableNoTracking
                 .Include(x => x.UploadedBy)
                 .Include(x => x.Bookings)
                 .ThenInclude(x => x.User)
                 .FirstOrDefaultAsync(x => x.Id == id);
        }
        
        public async Task UpdateAsync(Movie movie)
        {
            await _baseRepository.UpdateAsync(movie);
        }
        
        public async Task<List<Movie>> GetAllUpcomingAsync()
        {
            return await _baseRepository.Table
                .Where(x => x.IsActive == false && x.IsDeleted == false)
                .Include(x => x.UploadedBy)
                .ToListAsync();
        }
        
        public async Task<List<Movie>> GetAllDeletedAsync()
        {
            return await _baseRepository.Table
                .Where(x => x.IsDeleted == true)
                .Include(x => x.UploadedBy)
                .ToListAsync();
        }
        
        public async Task<Movie> GetWithUploaderAsync(int id)
        {
            return await _baseRepository.TableNoTracking
                .Include(x => x.UploadedBy)
                .SingleOrDefaultAsync(x => x.Id == id);
        }
        
        public async Task<List<Movie>> GetAllExpiredMoviesAsync()
        {
            return await _baseRepository.Table
                .Where(x => x.StartsAt.AddMinutes(x.DurationInMinutes) < DateTime.Now)
                .Include(x => x.Bookings)
                .Include(x => x.UploadedBy)
                .ToListAsync();
        }
    }
}
