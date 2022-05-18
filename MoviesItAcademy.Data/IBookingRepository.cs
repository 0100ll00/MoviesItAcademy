using MoviesItAcademy.Domain.Pocos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MoviesItAcademy.Data
{
    public interface IBookingRepository
    {
        Task BookAMovieAsync(Booking booking);
        Task BuyATicketAsync(Booking booking);
        Task UnbookAMovieAsync(Booking booking);
        Task<Booking> GetABookingAsync(int movieId, int userId);
        Task<List<Booking>> GetMoviesBookingsAsync(int movieId);
        Task<List<Booking>> GetAllExpiredBookingsAsync();
        Task<bool> IsMovieBookedAsync(int movieId, int userId);
        Task<bool> IsTicketBoughtAsync(int movieId, int userId);
        Task<List<Booking>> GetUsersBookingsAsync(int userId);
        Task<int> GetMovieBookCountAsync(int movieId);
        Task UpdateABookingAsync(Booking booking);
        Task<bool> ExistsAsync(int movieId, int userId);
    }
}
