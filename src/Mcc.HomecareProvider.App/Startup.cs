using System;
using Mcc.HomecareProvider.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Mcc.HomecareProvider
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Mcc.HomecareProvider", Version = "v1"});
            });

            var dbString =
                "Username=postgres;Password=postgres;Host=localhost;Port=5432;Database=hcp-db;Pooling=true;Keepalive=5;Command Timeout=60;";
            services
                .AddDbContext<PostgresDbContext>(
                    opt => opt.UseNpgsql(dbString),
                    ServiceLifetime.Scoped,
                    ServiceLifetime.Singleton);

            services
                .AddSingleton<Func<PostgresDbContext>>(
                    provider => () => new PostgresDbContext(
                        provider.GetRequiredService<DbContextOptions<PostgresDbContext>>()));
            
            services.AddScoped<PostgresDbContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mcc.HomecareProvider v1"));
            }

            app.UseRouting();

            app.UseAuthorization();
            
            IServiceProvider container = app.ApplicationServices;
            RunMigration(container);

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private void RunMigration(IServiceProvider container)
        {
            using IServiceScope scope = container.CreateScope();
            var dbContextFactory = container.GetRequiredService<Func<PostgresDbContext>>();
            using PostgresDbContext context = dbContextFactory();
            context.Database.Migrate();
        }
    }
}