using System;
using System.Threading.Tasks;
using Mcc.HomecareProvider.App.Controllers;
using Mcc.HomecareProvider.Domain;
using Mcc.HomecareProvider.Persistence;

namespace Mcc.HomecareProvider.App.Services
{
    public class PatientService
    {
        private readonly PostgresDbContext _dbContext;
        private readonly DateTimeProvider _timeProvider;

        public PatientService(DateTimeProvider timeProvider, PostgresDbContext dbContext)
        {
            _timeProvider = timeProvider;
            _dbContext = dbContext;
        }

        public async Task<Guid> CreatePatient(CreatePatientDto dto)
        {
            var firstName = dto.FirstName.Length <= 10 ? dto.FirstName : dto.FirstName.Substring(0, 10);
            var lastName = dto.LastName.Length <= 10 ? dto.LastName : dto.LastName.Substring(0, 10);
            var patient = new Patient(
                dto.Email,
                // bug
                firstName,
                lastName,
                dto.DateOfBirth,
                _timeProvider.UtcNow);

            _dbContext.Patients.Add(patient);
            await _dbContext.SaveChangesAsync();

            return patient.Id;
        }
    }
}