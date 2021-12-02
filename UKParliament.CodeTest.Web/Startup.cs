#region Namespace References
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UKParliament.CodeTest.Data.Context;
using UKParliament.CodeTest.Pattern.Service;
using UKParliament.CodeTest.Pattern.Service.Interfaces;
using UKParliament.CodeTest.Services;
using Microsoft.OpenApi.Models;
using System.Linq;
using System.Reflection;
using System;
using System.IO;
using FluentValidation.AspNetCore;
#endregion

namespace UKParliament.CodeTest.Web
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
            services.AddControllersWithViews()
                .AddFluentValidation(v =>
                {
                    v.RegisterValidatorsFromAssemblyContaining<Startup>();
                    v.DisableDataAnnotationsValidation = true;
                });

            services.AddDbContext<PersonManagerContext>(op => op.UseInMemoryDatabase("PersonManager"));
            RegisterServices(services);
            RegisterSwagger(services);

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        /// <summary>
        /// Register services here.
        /// </summary>
        /// <param name="services"></param>
        private void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<DbContext, PersonManagerContext>();
            services.AddScoped<ISmartServiceFactory, SmartServiceFactory>();
            services.AddScoped<IPersonService, PersonService>();
        }

        private void RegisterSwagger(IServiceCollection services)
        {
            // Swagger Configuration
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new OpenApiInfo { Title = "Person Manager API", Version = "v1" });
                s.ResolveConflictingActions(api => api.First());

                //Locate the XML file being generated by ASP.NET...
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.XML";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                //... and tell Swagger to use those XML comments.
                s.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSwagger();
            app.UseSwaggerUI(e => e.SwaggerEndpoint("/swagger/v1/swagger.json", "Person Manager API v1"));

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501
                //spa.UseProxyToSpaDevelopmentServer("http://localhost:5000");
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}