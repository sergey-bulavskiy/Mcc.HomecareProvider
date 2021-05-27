using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Mcc.HomecareProvider.App.Middleware.Exceptions
{
    /// <summary>
    /// Represents an error that should be converted to a certain HTTP response.
    ///
    /// <see cref="HttpStatusCode.InternalServerError"/> should not be thrown
    /// via <see cref="IWebApiException"/>,
    /// because these exceptions will be logged as Warnings,
    /// in this case throw <see cref="System.InvalidOperationException"/>.
    ///
    /// Please check the class <see cref="WebApiBaseException"/> before you implement this interface.
    /// </summary>
    public interface IWebApiException
    {
        /// <summary>
        /// Gets an object that is written to the HTTP response.
        /// </summary>
        IActionResult Result { get; }
    }
}