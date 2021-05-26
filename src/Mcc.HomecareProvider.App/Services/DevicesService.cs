using System;
using Mcc.HomecareProvider.Persistence;

namespace Mcc.HomecareProvider.App.Services
{
    public class DevicesService
    {
        private readonly PostgresDbContext _pgContext;

        public DevicesService(PostgresDbContext pgContext)
        {
            _pgContext = pgContext;
        }

        public Guid CreateDevice(string serialNumber)
        {
            return Guid.NewGuid();
        }
    }
}