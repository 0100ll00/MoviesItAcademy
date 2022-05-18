using System;
using System.Collections.Generic;
using System.Text;

namespace MoviesItAcademy.Service.Exceptions
{
    public class IncorrectCredentialsException : Exception
    {
        public readonly string ErrorCode = "IncorrectCredentialsException";
        public const string ErrorMessage = "Incorrect username or password were given.";

        public IncorrectCredentialsException() : base(ErrorMessage) { }
    }
}
