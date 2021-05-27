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

        /// <summary>
        /// Endpoint for patient creation.
        /// Email should be unique, first and last names should be no longer than 50 chars each.
        /// </summary>
        /// <param name="createPatientDto">Dto with all needed information for patients creation.</param>
        /// <returns>Id of the patient, that was created.</returns>
        [HttpPost]
        public Task<Guid> CreatePatient([FromBody] CreatePatientDto createPatientDto)
        {
            return _patientService.CreatePatient(createPatientDto);
        }
    }

    /// <summary>
    /// Dto that contains all necessary information for patient creation.
    /// </summary>
    public class CreatePatientDto
    {
        /// <summary>
        /// Email of the patient, unique in the system.
        /// </summary>
        public string Email { get; set; }
        
        /// <summary>
        /// First name of the patient. 
        /// </summary>
        public string FirstName { get; set; }
        
        /// <summary>
        /// Last name of the patient. 
        /// </summary>
        public string LastName { get; set; }
        
        /// <summary>
        /// Patients date of birth.
        /// </summary>
        public DateTime DateOfBirth { get; set; }
    }
}