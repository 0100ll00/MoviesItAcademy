using MoviesItAcademy.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MoviesItAcademy.Service.Abstractions
{
    public interface IBookingService
    {
        Task BookAMovieAsync(int movieId, int userId, DateTime bookingTime);
        Task BuyATicketAsync(int movieId, int userId);
        Task UnbookAMovieAsync(int movieId, int userId);
        Task DeleteExpiredBookingsAsync();
        Task<bool> IsMovieBookedAsync(int movieId, int userId);
        Task<bool> IsTicketBoughtAsync(int movieId, int userId);
        Task<bool> DoesBookingExistAsync(int movieId, int userId);
        Task<bool> IsMovieBookableAsync(int movieId);
        Task<List<BookingServiceModel>> GetUsersBookingsAsync(int userId);
        Task<BookingServiceModel> GetUserBookingAsync(int movieId, int userId);
    }
}
