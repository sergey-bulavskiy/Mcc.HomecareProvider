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
            builder.Entity<Patient>().Property(p => p.FirstName).IsRequired();
            builder.Entity<Patient>().Property(p => p.LastName).IsRequired();
            builder.Entity<Patient>().Property(p => p.Email).IsRequired();
            builder.Entity<Patient>().HasIndex(p => p.Email).IsUnique();
        }
    }
}