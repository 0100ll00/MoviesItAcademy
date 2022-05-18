using System;
using System.Collections.Generic;
using System.Text;

namespace MoviesItAcademy.Service.Exceptions
{
    public class ApplicationUserIdNotFoundException : Exception
    {
        public readonly string ErrorCode = "ApplicationUserIdNotFoundException";
        public const string ErrorMessage = "No user found with id: ";

        public ApplicationUserIdNotFoundException(int userId) : base(ErrorMessage + userId.ToString()) { }
    }
}
