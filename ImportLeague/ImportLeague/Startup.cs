using ImportLeague.Repositories;
using ImportLeague.Repositories.Interfaces;
using ImportLeague.Services;
using ImportLeague.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using ImportLeague.Services.FootballDataApi;
using System.Reflection;
using System;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using ImportLeague.WebApi.ServiceExtensions;

namespace ImportLeague
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

            // Add framework services.
            services.AddDbContext<ImportLeagueContext>(options =>
                    options.UseSqlServer(Configuration["ConnectionString"],
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(typeof(ImportLeagueContext).GetTypeInfo().Assembly.GetName().Name);
                        //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                    }));

            services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy())
                .AddSqlServer(Configuration["ConnectionString"],
                    name: "ImportLeagueDB-check",
                    tags: new string[] { "ImportLeagueDB" });

            services.Configure<FootballDataApiSettings>(Configuration.GetSection(FootballDataApiSettings.SettingKey));

            services.AddControllers();

            services.AddSwaggerGen( c =>
            {
                c.EnableAnnotations();
            });

            services.ConfigureMapper();

            services.AddScoped<ILeagueService, LeagueService>();
            services.AddScoped<IFootballDataApiReader, FootballDataApiReader>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger()
             .UseSwaggerUI(c =>
             {
                 c.SwaggerEndpoint($"/swagger/v1/swagger.json", "ImportLeague.API V1");
             });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
