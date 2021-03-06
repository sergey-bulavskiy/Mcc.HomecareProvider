using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mcc.HomecareProvider.App.Middleware.Exceptions
{
    /// <summary>
    ///     Base Exception that implements <see cref="IWebApiException" />
    ///     and creates <see cref="Result" /> as <see cref="ObjectResult" /> containing
    ///     <see cref="ProblemDetails" /> with <see cref="ProblemDetails.Type" /> derived from the
    ///     exception type.
    /// </summary>
    public abstract class WebApiBaseException : ApplicationException, IWebApiException
    {
        private readonly string _detail;
        private readonly int _statusCode;
        private readonly string _title;
        private readonly string _type;

        /// <summary>
        ///     Creates a instance of <see cref="WebApiBaseException" />
        /// </summary>
        /// <param name="type">
        ///     The machine-understandable identifier (URI) of the problem.
        ///     This is assigned to <see cref="ProblemDetails.Type" />.
        ///     If an invalid URI is specified, it is ignored and 'about:blank' is used instead.
        /// </param>
        /// <param name="title">
        ///     The <see cref="ProblemDetails.Title" /> of the error response.
        ///     This should not change between different instances of the problem.
        /// </param>
        /// <param name="statusCode">
        ///     Response Status Code of the Exception.
        ///     <see cref="StatusCodes.Status500InternalServerError" /> should not be
        ///     thrown via <see cref="IWebApiException" />,
        ///     because these exceptions will be logged as Warnings,
        ///     in this case throw <see cref="System.InvalidOperationException" />
        /// </param>
        /// <param name="detail">
        ///     Additional human-readable details. This string may contain parameters that vary from
        ///     instance to instance of the error.
        ///     This is assigned to <see cref="ProblemDetails.Detail" />.
        /// </param>
        protected WebApiBaseException(
            string type,
            string title,
            int statusCode,
            string detail = null) : base($"{title} {detail}")
        {
            if (!Uri.TryCreate(type, UriKind.Absolute, out var _)) type = "about:blank";

            if (string.IsNullOrEmpty(title)) title = GetType().Name;

            _title = title;
            _type = type;
            _detail = detail;
            _statusCode = statusCode;
        }

        /// <inheritdoc />
        public IActionResult Result => new ObjectResult(
            new ProblemDetails
            {
                Type = _type,
                Status = _statusCode,
                Title = _title,
                Detail = _detail
            })
        {
            StatusCode = _statusCode,
            ContentTypes = {"application/problem+json", "application/problem+xml"}
        };
    }
}