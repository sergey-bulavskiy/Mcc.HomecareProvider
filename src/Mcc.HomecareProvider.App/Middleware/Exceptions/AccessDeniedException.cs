using Microsoft.AspNetCore.Http;

namespace Mcc.HomecareProvider.App.Middleware.Exceptions
{
    /// <summary>
    ///     Thrown when the user has insufficient permissions to access a resource and
    ///     the server should reply with 403.
    /// </summary>
    /// <remarks>
    ///     This is a generic exception that should be thrown when e.g. a custom permission check is made
    ///     in an application service (i.e. permissions are checked manually, not by an attribute on
    ///     a controller action method). It should not be used to reject cross-tenant access.
    ///     For that case use <seealso cref="TenantAccessDeniedException" />.
    /// </remarks>
    public class AccessDeniedException : WebApiBaseException
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AccessDeniedException" /> class.
        /// </summary>
        /// <param name="message">The resource-specific error message.</param>
        public AccessDeniedException(string message) : base(
            ErrorTypes.AccessDenied,
            "Access was denied to the requested resource.",
            StatusCodes.Status403Forbidden,
            message)
        {
        }
    }
}