using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Mcc.HomecareProvider.Persistence
{
    /// <summary>
    ///     This class is to allow running powershell EF commands from the project folder without
    ///     specifying Startup class (without triggering the whole startup during EF operations
    ///     like add/remove migrations).
    /// </summary>
    public class PostgresDesignTimeDbContextFactory
        : IDesignTimeDbContextFactory<PostgresDbContext>
    {
        public PostgresDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PostgresDbContext>();

            // The connection string should be the same as in appsettings.Development.json.
            // This is needed to support `dotnet ef migrations remove`.
            const string db = "Username=postgres;Password=postgres;Host=localhost;Port=5432;"
                              + "Database=core;Pooling=true;Keepalive=5;";
            optionsBuilder.UseNpgsql(db);

            return new PostgresDbContext(optionsBuilder.Options);
        }
    }
}