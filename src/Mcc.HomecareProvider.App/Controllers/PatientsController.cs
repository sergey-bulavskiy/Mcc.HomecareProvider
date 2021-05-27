using System;
using System.Threading.Tasks;
using Mcc.HomecareProvider.App.Services;
using Microsoft.AspNetCore.Mvc;

namespace Mcc.HomecareProvider.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly PatientService _patientService;
        public PatientsController(PatientService patientService)
        {
            _patientService = patientService;
        }
        
        [HttpPost]
        public Task<Guid> CreatePatient([FromBody] CreatePatientDto createPatientDto)
        {
            return _patientService.CreatePatient(createPatientDto);
        }
    }

    public class CreatePatientDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}