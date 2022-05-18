using System;
using System.Collections.Generic;
using System.Text;

namespace MoviesItAcademy.Service.Exceptions
{
    public class BookingNotFoundException : Exception
    {
        public readonly string ErrorCode = "BookingNotFoundException";
        public const string ErrorMessage = "Booking not found for (User Id, Movie Id): ";

        public BookingNotFoundException(int userId, int movieId) 
            : base($"{ErrorMessage}({userId}, {movieId})") { }
    }
}
