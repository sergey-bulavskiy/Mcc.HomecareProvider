using System;
using Mcc.HomecareProvider.App.Services;
using Microsoft.AspNetCore.Mvc;

namespace Mcc.HomecareProvider.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private DevicesService _devicesService;

        public DevicesController(DevicesService devicesService)
        {
            _devicesService = devicesService;
        }


        [HttpPost]
        public Guid CreateDevice([FromBody] string serialNumber)
        {
            return _devicesService.CreateDevice(serialNumber);
        }
        
    }
}