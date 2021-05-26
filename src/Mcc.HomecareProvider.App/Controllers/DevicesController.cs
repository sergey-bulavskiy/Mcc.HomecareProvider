using System;
using System.Threading.Tasks;
using Mcc.HomecareProvider.App.Services;
using Microsoft.AspNetCore.Mvc;

namespace Mcc.HomecareProvider.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private readonly DevicesService _devicesService;

        public DevicesController(DevicesService devicesService)
        {
            _devicesService = devicesService;
        }

        [HttpPost]
        public Task<Guid> CreateDevice([FromQuery] string serialNumber)
        {
            return _devicesService.CreateDevice(serialNumber);
        }
        
    }
}