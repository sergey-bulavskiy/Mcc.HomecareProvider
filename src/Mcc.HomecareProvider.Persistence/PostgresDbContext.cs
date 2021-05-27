using Mcc.HomecareProvider.Domain;
using Microsoft.EntityFrameworkCore;

namespace Mcc.HomecareProvider.Persistence
{
    public class PostgresDbContext : DbContext
    {
        public PostgresDbContext(DbContextOptions<PostgresDbContext> options) : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<DeviceBinding> DeviceBindings { get; set; }
        public DbSet<StatisticalDay> StatisticalDays { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Device>().HasIndex(x => x.SerialNumber).IsUnique();
            builder.Entity<Device>()
                .Property(e => e.Id)
                .ValueGeneratedNever();

            builder.Entity<DeviceBinding>()
                .Property(e => e.Id)
                .ValueGeneratedNever();
            builder.Entity<DeviceBinding>()
                .HasOne<Device>()
                .WithOne(d => d.CurrentBinding)
                .HasForeignKey<Device>(d => d.CurrentBindingId)
                .IsRequired(false);
            builder.Entity<DeviceBinding>()
                .HasOne<Patient>()
                .WithOne(p => p.CurrentBinding)
                .HasForeignKey<Patient>(p => p.CurrentBindingId)
                .IsRequired(false);

            builder.Entity<Patient>()
                .Property(e => e.Id)
                .ValueGeneratedNever();
            builder.Entity<Patient>()
                .HasMany(p => p.DeviceBindings)
                .WithOne(b => b.Patient)
                .HasForeignKey(b => b.PatientId)
                .IsRequired(false);
            builder.Entity<Patient>().Property(e => e.FirstName).IsRequired();
            builder.Entity<Patient>().Property(e => e.LastName).IsRequired();
            
            builder.Entity<StatisticalDay>()
                .Property(e => e.Id)
                .ValueGeneratedNever();
        }
    }
}