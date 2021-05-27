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
                using (IServiceScope scope = host.Services.CreateScope())
                {
                    IServiceProvider services = scope.ServiceProvider;
                    try
                    {
                        DbInitializer dbInitializer = services.GetRequiredService<DbInitializer>();
                        dbInitializer.Initialize();
                    }
                    catch (Exception ex)
                    {
                        var logger = services.GetRequiredService<ILogger<Program>>();
                        logger.LogError(ex, "An error occurred while initializing the database.");
                        throw;
                    }
                }
            }
            
            IHostBuilder builder = Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });

            IHost host = builder.Build();
            InitDb(host);
            
            host.Run();
        }
    }
}