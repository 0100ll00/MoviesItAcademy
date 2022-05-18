using System;
using System.Collections.Generic;
using System.Text;

namespace MoviesItAcademy.Service.Exceptions
{
    public class EmailAlreadyRegisteredException : Exception
    {
        public readonly string ErrorCode = "ApplicationUserNameNotFoundException";
        public const string ErrorMessage = "Such a username already exists: ";

        public EmailAlreadyRegisteredException(string username) : base(ErrorMessage + username) { }
    }
}
