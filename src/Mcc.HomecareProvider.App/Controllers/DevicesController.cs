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

        /// <summary>
        /// Endpoint for device creation.
        /// Serial number should contain 6 digits, and be within [100000; 999999] 
        /// </summary>
        /// <param name="serialNumber">Serial number of the device.
        /// Should contain 6 digits, and be within [100000; 999999] </param>
        /// <returns>Id of created device.</returns>
        [HttpGet]
        public Task<Guid> CreateDevice([FromQuery] string serialNumber)
        {
            return _devicesService.CreateDevice(serialNumber);
        }

        /// <summary>
        /// Endpoint for patient-device assignment.
        /// Need to specify two Ids - Patient's one, and Device's one. 
        /// </summary>
        /// <param name="dto">Dto that contains all needed info for assignment.</param>
        /// <returns>Id of the Device Binding that was created because of assignment.</returns>
        [HttpPut]
        [Route("assign")]
        public Task<Guid> AssignDeviceToPatient([FromBody] AssignDeviceToPatientDto dto)
        {
            return _devicesService.AssignDeviceToPatient(dto);
        }
    }

    /// <summary>
    /// Dto that is required for device assignment operation.
    /// </summary>
    public class AssignDeviceToPatientDto
    {
        /// <summary>
        /// Id of the patient that is being assigned to device.
        /// </summary>
        public Guid PatientId { get; set; }

        /// <summary>
        /// Id of the device that is being assigned to patient.
        /// </summary>
        public Guid DeviceId { get; set; }
    }
}