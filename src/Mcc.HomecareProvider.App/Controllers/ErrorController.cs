using Microsoft.AspNetCore.Mvc;

namespace Mcc.HomecareProvider.App.Controllers
{
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [Route("/error")]
        protected IActionResult Error() => Problem();
    }
}