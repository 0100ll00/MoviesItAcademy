using System;
using System.Collections.Generic;
using System.Text;

namespace MoviesItAcademy.Service.Exceptions
{
    public class ApplicationUsernameNotFoundException : Exception
    {
        public readonly string ErrorCode = "ApplicationUserNameNotFoundException";
        public const string ErrorMessage = "No user found with username: ";

        public ApplicationUsernameNotFoundException(string username) : base(ErrorMessage + username) { }
    }
}
