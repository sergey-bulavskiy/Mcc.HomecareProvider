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

        [HttpGet]
        public Task<Guid> CreateDevice([FromQuery] string serialNumber)
        {
            return _devicesService.CreateDevice(serialNumber);
        }

        [HttpPut]
        [Route("assign")]
        public Task<Guid> AssignDeviceToPatient([FromBody] AssignDeviceToPatientDto dto)
        {
            return _devicesService.AssignDeviceToPatient(dto);
        }
    }

    public class AssignDeviceToPatientDto
    {
        public Guid PatientId { get; set; }
        public Guid DeviceId { get; set; }
    }
}