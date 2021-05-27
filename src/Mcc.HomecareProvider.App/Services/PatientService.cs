using System;
using System.Threading.Tasks;
using Mcc.HomecareProvider.App.Controllers;
using Mcc.HomecareProvider.Domain;
using Mcc.HomecareProvider.Persistence;

namespace Mcc.HomecareProvider.App.Services
{
    public class PatientService
    {
        private readonly DateTimeProvider _timeProvider;
        private readonly PostgresDbContext _dbContext;

        public PatientService(DateTimeProvider timeProvider, PostgresDbContext dbContext)
        {
            _timeProvider = timeProvider;
            _dbContext = dbContext;
        }

        public async Task<Guid> CreatePatient(CreatePatientDto dto)
        {
            var patient = new Patient(
                email: dto.Email,
                // bug
                firstName: dto.FirstName.Substring(0, 10),
                lastName: dto.LastName.Substring(0, 10),
                dateOfBirth: dto.DateOfBirth,
                createdAt: _timeProvider.UtcNow);

            _dbContext.Patients.Add(patient);
            await _dbContext.SaveChangesAsync();

            return patient.Id;
        }
    }
}