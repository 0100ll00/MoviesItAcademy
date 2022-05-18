using System;
using System.Collections.Generic;
using System.Text;

namespace MoviesItAcademy.Service.Exceptions
{
    public class BookingAvailabilityHasExpiredException : Exception
    {
        public readonly string ErrorCode = "BookingAvailabilityHasExpiredException";
        public const string ErrorMessage = "Movie cannot be booked after an hour before seance: ";

        public BookingAvailabilityHasExpiredException(string title) : base(ErrorMessage + title) { }
    }
}
