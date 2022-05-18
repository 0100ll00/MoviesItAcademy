using System;
using System.Collections.Generic;
using System.Text;

namespace MoviesItAcademy.Service.Exceptions
{
    public class MovieIdAlreadyExistsException : Exception
    {
        public readonly string ErrorCode = "MovieIdAlreadyExistsException";
        public const string ErrorMessage = "Movie with a given id already exists: ";

        public MovieIdAlreadyExistsException(int id) : base(ErrorMessage + id.ToString()) { }
    }
}
