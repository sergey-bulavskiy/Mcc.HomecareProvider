using System;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mcc.HomecareProvider.App.Controllers
{
    public class VersionController
    {
        /// <summary>
        /// Gets the version of the service.
        /// </summary>
        /// <returns>A string representing the version.</returns>
        [AllowAnonymous]
        [HttpGet("api")]
        public string Version()
        {
            var attribute = typeof(VersionController).GetTypeInfo()
                .Assembly
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>();

            if (attribute == null)
                throw new Exception("Can not retrieve api version");

            return attribute.InformationalVersion;
        }

        [HttpGet("/")]
        [AllowAnonymous]
        public ActionResult RedirectToSwagger()
        {
            return new RedirectResult("swagger");
        }
    }
}