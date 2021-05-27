using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mcc.HomecareProvider.App.Middleware.Exceptions
{
    /// <summary>
    /// Thrown when the client provides an invalid input and the
    /// server should respond with a 400 status.
    /// </summary>
    public class ValidationException : ApplicationException, IWebApiException
    {
        private readonly ValidationProblemDetails _details;

        public ValidationException(string message) : base(message)
        {
            _details = new ValidationProblemDetails
            {
                Type = ErrorTypes.ValidationError,
                Detail = message
            };
        }

        public ValidationException(IDictionary<string, string[]> errors)
            : base(ErrorsToString(errors))
        {
            _details = new ValidationProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Type = ErrorTypes.ValidationError
            };
            foreach (KeyValuePair<string, string[]> e in errors)
            {
                _details.Errors[e.Key] = e.Value;
            }
        }

        /// <inheritdoc />
        public IActionResult Result => new ObjectResult(_details)
        {
            StatusCode = StatusCodes.Status400BadRequest,
            ContentTypes = { "application/problem+json" }
        };

        private static string ErrorsToString(IDictionary<string, string[]> errors)
        {
            IEnumerable<string> errs =
                from kv in errors
                from v in kv.Value
                select $"{kv.Key}: {v}";
            return string.Join("\n", errs);
        }
    }
}