using System;
using System.Collections.Generic;
using System.Text;

namespace MoviesItAcademy.Service.Exceptions
{
    public class MovieIdNotFoundException : Exception
    {
        public readonly string ErrorCode = "MovieIdNotFoundException";
        public const string ErrorMessage = "No movie found with id: ";

        public MovieIdNotFoundException(int id) : base(ErrorMessage + id.ToString()) { }
    }
}
