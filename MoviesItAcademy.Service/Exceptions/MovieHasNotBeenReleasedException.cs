using System;
using System.Collections.Generic;
using System.Text;

namespace MoviesItAcademy.Service.Exceptions
{
    public class MovieHasNotBeenReleasedException : Exception
    {
        public readonly string ErrorCode = "MovieHasNotBeenReleasedException";
        public const string ErrorMessage = "Movie has not yet been released: ";

        public MovieHasNotBeenReleasedException(string title) : base(ErrorMessage + title) { }
    }
}
