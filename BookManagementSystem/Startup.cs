using System;
using System.Linq;
using System.Net;
using System.Reflection;
using Autofac;
using BookManagementSystem.Domain.Book;
using BookManagementSystem.Factories;
using BookManagementSystem.Infrastructure.Domain;
using BookManagementSystem.Services;
using BookManagementSystem.Storage.Database;
using BookManagementSystem.Storage.Events;
using BookManagementSystem.Validators;
using EventStore.ClientAPI;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BookManagementSystem
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options
                    .UseLazyLoadingProxies()
                    .UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddControllersWithViews();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.Register(ctx => EventStoreConnectionFactory.Create()).As<IEventStoreConnection>().SingleInstance().AsSelf();
            builder.RegisterGeneric(typeof(DomainObjectRepository<,,>)).AsSelf();
            builder.RegisterType<EventsRepository>().AsImplementedInterfaces().AsSelf();
            builder.RegisterType<BookCommandsHandler>().AsImplementedInterfaces();
            builder.RegisterType<BookCommandsValidator>().AsImplementedInterfaces();
            builder.RegisterType<ReadModelUpdateService>().AsImplementedInterfaces();
            builder.RegisterType<Mediator>().As<IMediator>().InstancePerLifetimeScope();

            builder.RegisterType<ApplicationDbContext>().AsSelf().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWork>().AsSelf().AsImplementedInterfaces().ExternallyOwned();
            builder.RegisterGeneric(typeof(ReadDatabaseRepository<,>)).As(typeof(IReadDatabaseRepository<,>)).InstancePerLifetimeScope();


            builder.Register<ServiceFactory>(context =>
            {
                var c = context.Resolve<IComponentContext>();
                return t =>
                {
                    var resolve = c.Resolve(t);
                    return resolve;
                };
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

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
