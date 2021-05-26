using System;
using System.Threading.Tasks;
using Mcc.HomecareProvider.Domain;
using Mcc.HomecareProvider.Persistence;

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
            var device = new Device(serialNumber, _timeProvider.UtcNow);

            _dbContext.Devices.Add(device);
            await _dbContext.SaveChangesAsync();

            device.InitializeCurrentDeviceBinding();
            await _dbContext.SaveChangesAsync();

            return device.Id;
        }
    }
}