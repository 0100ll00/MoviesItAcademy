using System;
using System.Collections.Generic;
using System.Text;

namespace MoviesItAcademy.Service.Exceptions
{
    public class MovieNoTicketsException : Exception
    {
        public readonly string ErrorCode = "MovieNoTicketsException";
        public const string ErrorMessage = "Movie tickets have already been sold out for: ";

        public MovieNoTicketsException(string title) : base(ErrorMessage + title) { }
    }
}
