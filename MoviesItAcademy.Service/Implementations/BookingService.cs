using Mapster;
using MoviesItAcademy.Data;
using MoviesItAcademy.Domain.Pocos;
using MoviesItAcademy.Service.Abstractions;
using MoviesItAcademy.Service.Exceptions;
using MoviesItAcademy.Service.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoviesItAcademy.Service.Implementations
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly IMovieRepository _movieRepository;

        public BookingService(IBookingRepository bookingRepository,
                              IApplicationUserRepository applicationUserRepository,
                              IMovieRepository movieRepository)
        {
            _bookingRepository = bookingRepository;
            _applicationUserRepository = applicationUserRepository;
            _movieRepository = movieRepository;
        }

        public async Task BookAMovieAsync(int movieId, int userId, DateTime bookingTime)
        {
            if (await _bookingRepository.ExistsAsync(movieId, userId))
                return;

            var movie = await _movieRepository.GetAsync(movieId);
            if (movie == null)
                throw new MovieIdNotFoundException(movieId);
            if (movie.IsDeleted == true)
                throw new MovieHasBeenDeletedException(movie.Title);
            if (movie.IsActive == false)
                throw new MovieHasNotBeenReleasedException(movie.Title);
            if (movie.StartsAt.AddHours(-1) < DateTime.Now)
                throw new BookingAvailabilityHasExpiredException(movie.Title);

            var movieTicketTotal = movie.TotalSeats;
            var movieAvailableCount = movieTicketTotal - (await _bookingRepository.GetMovieBookCountAsync(movieId));

            if (movieAvailableCount <= 0)
                throw new MovieNoTicketsException(movie.Title);

            var booking = new Booking()
            {
                MovieId = movieId,
                UserId = userId,
                IsActive = false,
                BookedAt = bookingTime,
                StartsAt = movie.StartsAt,
                RunsOutAt = movie.StartsAt.AddHours(-1)
            };
            await _bookingRepository.BookAMovieAsync(booking);
        }

        public async Task BuyATicketAsync(int movieId, int userId)
        {
            var movie = await _movieRepository.GetAsync(movieId);
            if (movie == null)
                throw new MovieIdNotFoundException(movieId);
            if (movie.StartsAt.AddMinutes(movie.DurationInMinutes) < DateTime.Now)
                throw new MovieNoTicketsException(movie.Title);
            if (movie.IsDeleted == true)
                throw new MovieHasBeenDeletedException(movie.Title);
            if (movie.IsActive == false)
                throw new MovieHasNotBeenReleasedException(movie.Title);

            var booking = await _bookingRepository.GetABookingAsync(movieId, userId);
            if (booking == null)
            {
                var movieTicketTotal = movie.TotalSeats;
                var movieAvailableCount = movieTicketTotal - (await _bookingRepository.GetMovieBookCountAsync(movieId));

                if (movieAvailableCount <= 0)
                    throw new MovieNoTicketsException(movie.Title);

                booking = new Booking()
                {
                    MovieId = movieId,
                    UserId = userId,
                    IsActive = true,
                    BookedAt = DateTime.Now,
                    StartsAt = movie.StartsAt,
                    RunsOutAt = movie.StartsAt
                };
                await _bookingRepository.BookAMovieAsync(booking);
            }
            else
            {
                booking.IsActive = true;
                booking.RunsOutAt = booking.StartsAt;
                await _bookingRepository.BuyATicketAsync(booking);
            }
        }

        public async Task<bool> DoesBookingExistAsync(int movieId, int userId)
        {
            return await _bookingRepository.ExistsAsync(movieId, userId);
        }

        public async Task DeleteExpiredBookingsAsync()
        {
            var bookings = await _bookingRepository.GetAllExpiredBookingsAsync();
            foreach (var booking in bookings)
                await _bookingRepository.UnbookAMovieAsync(booking);
        }

        public async Task<List<BookingServiceModel>> GetUsersBookingsAsync(int userId)
        {
            if (!await _applicationUserRepository.ExistsAsync(userId))
                throw new ApplicationUserIdNotFoundException(userId);

            var bookings = await _bookingRepository.GetUsersBookingsAsync(userId);
            var bookingsServiceModel = bookings.Adapt<List<BookingServiceModel>>();
            return bookingsServiceModel;
        }

        public async Task<bool> IsMovieBookedAsync(int movieId, int userId)
        {
            return await _bookingRepository.IsMovieBookedAsync(movieId, userId);
        }

        public async Task<bool> IsTicketBoughtAsync(int movieId, int userId)
        {
            return await _bookingRepository.IsTicketBoughtAsync(movieId, userId);
        }

        public async Task UnbookAMovieAsync(int movieId, int userId)
        {
            if (!await _bookingRepository.ExistsAsync(movieId, userId))
                throw new BookingNotFoundException(userId, movieId);

            var booking = new Booking() { MovieId = movieId, UserId = userId };
            await _bookingRepository.UnbookAMovieAsync(booking);
        }

        public async Task<BookingServiceModel> GetUserBookingAsync(int movieId, int userId)
        {
            if (!await _applicationUserRepository.ExistsAsync(userId))
                throw new ApplicationUserIdNotFoundException(userId);

            var booking = await _bookingRepository.GetABookingAsync(movieId, userId);
            if (booking == null)
                throw new BookingNotFoundException(userId, movieId);
            var bookingServiceModel = booking.Adapt<BookingServiceModel>();
            return bookingServiceModel;
        }

        public async Task<bool> IsMovieBookableAsync(int movieId)
        {
            var movie = await _movieRepository.GetAsync(movieId);
            var movieTicketTotal = movie.TotalSeats;
            var movieAvailableCount = movieTicketTotal - (await _bookingRepository.GetMovieBookCountAsync(movieId));

            if (movieAvailableCount <= 0)
                return false;
            return true;
        }
    }
}
