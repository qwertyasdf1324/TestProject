using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using NHibernate;
using System.Data;
using System.Globalization;
using Environment = NHibernate.Cfg.Environment;

namespace SimpleWebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public static ISessionFactory SessionFactory;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var allAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            var myAssembly = allAssemblies
                .Where(a => a.GetName().Name.Contains("SimpleWebApi")).FirstOrDefault();

            var cfg = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012.ConnectionString(Configuration.GetConnectionString("myConnectionString")))
                .ExposeConfiguration(c =>
                {
                    c.SetProperty(Environment.Isolation, "ReadUncommitted");
                    c.SetProperty(Environment.GenerateStatistics, "true");
                    c.SetProperty(Environment.ShowSql, "true");
                    c.SetProperty(Environment.UseSecondLevelCache, "false");
                    c.SetProperty(Environment.UseQueryCache, "false");
                    c.SetProperty(Environment.CommandTimeout,
                    TimeSpan.FromSeconds(30).TotalSeconds.ToString(CultureInfo.InvariantCulture));
                })
                .Mappings(_ =>
                {
                    _.FluentMappings
                    .Conventions.Setup(x => x.Add(AutoImport.Never()))
                    .AddFromAssembly(myAssembly);
                });

            SessionFactory = cfg.BuildConfiguration().BuildSessionFactory();

            services.AddSingleton(SessionFactory);
            services.AddScoped(factory => SessionFactory.OpenSession());

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
