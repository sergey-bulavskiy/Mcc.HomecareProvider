using Mcc.HomecareProvider.Domain;
using Microsoft.EntityFrameworkCore;

namespace Mcc.HomecareProvider.Persistence
{
    public class PostgresDbContext : DbContext
    {
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<DeviceBinding> DeviceBindings { get; set; }
        public DbSet<StatisticalDay> StatisticalDays { get; set; }
    }
}
