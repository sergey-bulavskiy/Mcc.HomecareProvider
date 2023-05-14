using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Mcc.HomecareProvider.App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            void InitDb(IHost host)
            {
                using var scope = host.Services.CreateScope();
                var services = scope.ServiceProvider;
                try
                {
                    var dbInitializer = services.GetRequiredService<DbInitializer>();
                    dbInitializer.Initialize();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while initializing the database.");
                    throw;
                }
            }

            var builder = Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });

            var host = builder.Build();
            InitDb(host);

            host.Run();
        }
    }
}