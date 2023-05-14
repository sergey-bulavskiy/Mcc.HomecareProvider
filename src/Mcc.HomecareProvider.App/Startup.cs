using System;
using System.IO;
using Mcc.HomecareProvider.App;
using Mcc.HomecareProvider.App.Middleware;
using Mcc.HomecareProvider.App.Services;
using Mcc.HomecareProvider.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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
            // Set Global JSON Converter. Need for function JsonConvert.SerializeObject
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            services.AddControllers()
                .AddNewtonsoftJson(
                    setupAction =>
                    {
                        setupAction.SerializerSettings.ReferenceLoopHandling =
                            ReferenceLoopHandling.Ignore;
                        setupAction.SerializerSettings.ContractResolver =
                            new CamelCasePropertyNamesContractResolver();
                    });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Mcc.HomecareProvider", Version = "v1"});
                string basePath = AppContext.BaseDirectory;
                c.IncludeXmlComments(Path.Combine(basePath, "Mcc.HomecareProvider.App.xml"));
            });

            var dbString = Configuration.GetConnectionString("DefaultConnection");
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
            services.AddScoped<DateTimeProvider>();
            services.AddScoped<DevicesService>();
            services.AddScoped<PatientService>();
            services.AddScoped<DbInitializer>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mcc.HomecareProvider v1"));
            app.UseRouting();

            app.UseAuthorization();
            app.UseMiddleware<ErrorHandlerMiddleware>();

            // var container = app.ApplicationServices;
            //RunMigration(container);

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private void RunMigration(IServiceProvider container)
        {
            using var scope = container.CreateScope();
            var dbContextFactory = container.GetRequiredService<Func<PostgresDbContext>>();
            using var context = dbContextFactory();
            context.Database.Migrate();
        }
    }
}