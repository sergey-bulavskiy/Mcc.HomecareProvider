using System;
using System.Threading.Tasks;
using Mcc.HomecareProvider.App.Controllers;
using Mcc.HomecareProvider.App.Middleware.Exceptions;
using Mcc.HomecareProvider.Domain;
using Mcc.HomecareProvider.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Mcc.HomecareProvider.App.Services
{
    public class DevicesService
    {
        private readonly PostgresDbContext _dbContext;
        private readonly DateTimeProvider _timeProvider;

        public DevicesService(PostgresDbContext dbContext, DateTimeProvider timeProvider)
        {
            _dbContext = dbContext;
            _timeProvider = timeProvider;
        }

        public async Task<Guid> CreateDevice(string serialNumber)
        {
            var isDeviceExists = await _dbContext.Devices.AnyAsync(d => d.SerialNumber == serialNumber);
            if (isDeviceExists)
                throw new ValidationException($"Device with such serial number: {serialNumber} already exists.");

            var device = new Device(serialNumber, _timeProvider.UtcNow);

            _dbContext.Devices.Add(device);
            await _dbContext.SaveChangesAsync();

            device.InitializeCurrentDeviceBinding();
            await _dbContext.SaveChangesAsync();

            return device.Id;
        }

        public async Task<Guid> AssignDeviceToPatient(AssignDeviceToPatientDto dto)
        {
            var patient = await _dbContext.Patients.SingleOrDefaultAsync(p => p.Id == dto.PatientId);
            if (patient is null)
            {
                throw new ResourceNotFoundException(
                    $"Given {nameof(dto.PatientId)}: {dto.PatientId} is not found in the database.");
            }

            var device = await _dbContext.Devices.SingleOrDefaultAsync(d => d.Id == dto.DeviceId);
            if (device is null)
            {
                throw new ResourceNotFoundException(
                    $"Given {nameof(dto.DeviceId)}: {dto.DeviceId} is not found in the database.");
            }

            var newBinding = device.AssignToPatient(patient, _timeProvider.UtcNow);
            await _dbContext.SaveChangesAsync();

            return newBinding.Id;
        }
    }
}