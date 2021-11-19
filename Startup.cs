using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Npgsql;
using pfm.Database;
using pfm.Database.Repositories;
using pfm.Services;

namespace pfm
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "pfm", Version = "v1" });
            });
            services.AddDbContext<PfmDbContext>(options=>{
                options.UseNpgsql(CreateConnectionsString());
            });
            services.AddScoped<IPfmRepository, PfmRepository>();
            services.AddScoped<IPfmService, PfmService>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "pfm v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        private string CreateConnectionsString()
        {
            var username = Environment.GetEnvironmentVariable("DATABASE_USERNAME") ?? this.Configuration["Database:Username"];
            var password = Environment.GetEnvironmentVariable("DATABASE_PASSWORD") ?? this.Configuration["Database:Password"];
            var host = Environment.GetEnvironmentVariable("DATABASE_HOST") ?? this.Configuration["Database:Host"];
            var port = Environment.GetEnvironmentVariable("DATABASE_PORT") ?? this.Configuration["Database:Port"];
            var database = Environment.GetEnvironmentVariable("DATABASE_NAME") ?? this.Configuration["Database:Name"];

            var builder = new NpgsqlConnectionStringBuilder()
            {
                Username = username,
                Password = password,
                Host = host,
                Port = int.Parse(port),
                Database = database,
                Pooling = true,
            };
            return builder.ConnectionString;
        }
    }
}
