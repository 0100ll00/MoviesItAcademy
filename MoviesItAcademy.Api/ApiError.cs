using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MoviesItAcademy.Service.Exceptions;
using System;
using System.Net;

namespace MoviesItAcademy.Api
{
    public class ApiError : ProblemDetails
    {
        public const string UnhandledError = "UnhandledException";
        private HttpContext _context;
        private Exception _exception;

        public LogLevel LogLevel { get; set; }
        public string Code { get; set; }
        public string TraceId
        {
            get
            {
                if (Extensions.TryGetValue("TraceId", out var traceId))
                    return (string)traceId;

                return null;
            }
            set => Extensions["TraceId"] = value;
        }

        public ApiError(HttpContext context, Exception exception)
        {
            _context = context;
            _exception = exception;

            TraceId = context.TraceIdentifier;
            Code = "UnhandledErrorCode";
            Title = exception.Message;
            LogLevel = LogLevel.Error;
            Instance = context.Request.Path;
            Status = Status = (int)HttpStatusCode.InternalServerError;

            HandleException((dynamic)exception);
        }

        private void HandleException(ApplicationUserIdNotFoundException exception)
        {
            Code = exception.ErrorCode;
            Status = (int)HttpStatusCode.NotFound;
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4";
            Title = exception.Message;
            LogLevel = LogLevel.Trace;
        }

        private void HandleException(ApplicationUsernameNotFoundException exception)
        {
            Code = exception.ErrorCode;
            Status = (int)HttpStatusCode.NotFound;
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4";
            Title = exception.Message;
            LogLevel = LogLevel.Information;
        }

        private void HandleException(BookingNotFoundException exception)
        {
            Code = exception.ErrorCode;
            Status = (int)HttpStatusCode.NotFound;
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4";
            Title = exception.Message;
            LogLevel = LogLevel.Information;
        }

        private void HandleException(BookingAvailabilityHasExpiredException exception)
        {
            Code = exception.ErrorCode;
            Status = (int)HttpStatusCode.NotAcceptable;
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4";
            Title = exception.Message;
            LogLevel = LogLevel.Information;
        }

        private void HandleException(EmailAlreadyRegisteredException exception)
        {
            Code = exception.ErrorCode;
            Status = (int)HttpStatusCode.Conflict;
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4";
            Title = exception.Message;
            LogLevel = LogLevel.Trace;
        }

        private void HandleException(IncorrectCredentialsException exception)
        {
            Code = exception.ErrorCode;
            Status = (int)HttpStatusCode.Unauthorized;
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.6";
            Title = exception.Message;
            LogLevel = LogLevel.Information;
        }

        private void HandleException(MovieHasBeenDeletedException exception)
        {
            Code = exception.ErrorCode;
            Status = (int)HttpStatusCode.NotFound;
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8";
            Title = exception.Message;
            LogLevel = LogLevel.Information;
        }

        private void HandleException(MovieHasNotBeenReleasedException exception)
        {
            Code = exception.ErrorCode;
            Status = (int)HttpStatusCode.NotFound;
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8";
            Title = exception.Message;
            LogLevel = LogLevel.Information;
        }

        private void HandleException(MovieIdAlreadyExistsException exception)
        {
            Code = exception.ErrorCode;
            Status = (int)HttpStatusCode.Conflict;
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8";
            Title = exception.Message;
            LogLevel = LogLevel.Information;
        }

        private void HandleException(MovieIdNotFoundException exception)
        {
            Code = exception.ErrorCode;
            Status = (int)HttpStatusCode.NotFound;
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4";
            Title = exception.Message;
            LogLevel = LogLevel.Information;
        }

        private void HandleException(MovieNoTicketsException exception)
        {
            Code = exception.ErrorCode;
            Status = (int)HttpStatusCode.NotAcceptable;
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.6";
            Title = exception.Message;
            LogLevel = LogLevel.Information;
        }
    }
}
