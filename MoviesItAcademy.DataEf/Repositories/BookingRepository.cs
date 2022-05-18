using Microsoft.EntityFrameworkCore;
using MoviesItAcademy.Data;
using MoviesItAcademy.Domain.Pocos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesItAcademy.DataEf.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private IBaseRepository<Booking> _baseRepository;

        public BookingRepository(IBaseRepository<Booking> baseRepository)
        {
            _baseRepository = baseRepository;
        }

        public async Task BookAMovieAsync(Booking booking)
        {
            await _baseRepository.AddAsync(booking);
        }

        public async Task BuyATicketAsync(Booking booking)
        {
            await _baseRepository.UpdateAsync(booking);
        }

        public async Task<bool> ExistsAsync(int movieId, int userId)
        {
            return await _baseRepository.AnyAsync(x => x.MovieId == movieId && x.UserId == userId);
        }

        public async Task<Booking> GetABookingAsync(int movieId, int userId)
        {
            return await _baseRepository.Table
                .Where(x => x.UserId == userId && x.MovieId == movieId)
                .Include(x => x.Movie)
                .SingleOrDefaultAsync();
        }

        public async Task<List<Booking>> GetAllExpiredBookingsAsync()
        {
            return await _baseRepository.Table
                .Where(x => x.RunsOutAt < DateTime.Now)
                .ToListAsync();
        }

        public async Task<int> GetMovieBookCountAsync(int movieId)
        {
            return await _baseRepository.Table
                .Where(x => x.MovieId == movieId)
                .CountAsync();
        }

        public async Task<List<Booking>> GetMoviesBookingsAsync(int movieId)
        {
            return await _baseRepository.Table
                .Where(x => x.MovieId == movieId)
                .ToListAsync();
        }

        public async Task<List<Booking>> GetUsersBookingsAsync(int userId)
        {
            return await _baseRepository.Table
                .Where(x => x.UserId == userId)
                .Include(x => x.Movie)
                .ToListAsync();
        }

        public async Task<bool> IsMovieBookedAsync(int movieId, int userId)
        {
            return await _baseRepository.Table
                .Where(x => x.UserId == userId && x.MovieId == movieId)
                .AnyAsync();
        }

        public async Task<bool> IsTicketBoughtAsync(int movieId, int userId)
        {
            return await _baseRepository.Table
                .Where(x => x.UserId == userId && x.MovieId == movieId && x.IsActive == true)
                .AnyAsync();
        }

        public async Task UpdateABookingAsync(Booking booking)
        {
            await _baseRepository.UpdateAsync(booking);
        }

        public async Task UnbookAMovieAsync(Booking booking)
        {
            await _baseRepository.RemoveAsync(booking);
        }
    }
}
