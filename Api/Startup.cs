using Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Models;
using Repository;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DexProjectService
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
            services.AddDbContext<Context>(options => options.UseInMemoryDatabase("ProjectServiceDB"));;
            
            services.AddScoped<ProjectRepository>();
            services.AddScoped<ProjectService>();
            services.AddScoped<EventService>();

            SeedData(services);

            services.AddControllers();
            services.AddSwaggerGen();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("swagger/v1/swagger.json", "My API V1"); c.RoutePrefix = string.Empty; });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public void SeedData(IServiceCollection services)
        {
            var context = services.BuildServiceProvider()
                       .GetService<Context>();

            context.Project.Add(new Project
            {
                Id = 1,
                Name = "Project 1",
                Description = "Dit is de beschrijving"
            });

            context.Project.Add(new Project
            {
                Id = 2,
                Name = "Project 3",
                Description = "Dit is de beschrijving"
            });

            context.Project.Add(new Project
            {
                Id = 3,
                Name = "Project 2",
                Description = "Dit is de beschrijving"
            });

            context.SaveChanges();
        }
    }
}
