using System;
using System.Collections.Generic;
using System.Text;

namespace MoviesItAcademy.Service.Exceptions
{
    public class MovieHasBeenDeletedException : Exception
    {
        public readonly string ErrorCode = "MovieHasBeenDeletedException";
        public const string ErrorMessage = "Movie has been moved to archive: ";

        public MovieHasBeenDeletedException(string title) : base(ErrorMessage + title) { }
    }
}
