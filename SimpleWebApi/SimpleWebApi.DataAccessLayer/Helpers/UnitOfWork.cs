using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using Microsoft.Extensions.Configuration;
using NHibernate;
using NHibernate.Context;
using System;
using System.Globalization;
using System.Linq;
using Environment = NHibernate.Cfg.Environment;

namespace SimpleWebApi.DataAccessLayer.Helpers
{
    public class UnitOfWork : IUnitOfWork
    {
        IConfiguration configuration;
        public ITransaction Transaction { get; set; }
        public ISession Session => SessionFactory.OpenSession();
        
        public ISessionFactory SessionFactory
        {
            get
            {
                var allAssemblies = AppDomain.CurrentDomain.GetAssemblies();
                var myAssembly = allAssemblies
                    .Where(a => a.GetName().Name.Contains("SimpleWebApi.DataAccessLayer")).FirstOrDefault();

                var cfg = Fluently.Configure()
                    .Database(MsSqlConfiguration.MsSql2012.ConnectionString(configuration.GetConnectionString("myConnectionString")))
                    .ExposeConfiguration(c =>
                    {
                        c.SetProperty(Environment.Isolation, "ReadUncommitted");
                        c.SetProperty(Environment.GenerateStatistics, "true");
                        c.SetProperty(Environment.ShowSql, "true");
                        c.SetProperty(Environment.UseSecondLevelCache, "false");
                        c.SetProperty(Environment.UseQueryCache, "false");
                        c.SetProperty("current_session_context_class", "web");
                        c.SetProperty(Environment.CommandTimeout,
                        TimeSpan.FromSeconds(30).TotalSeconds.ToString(CultureInfo.InvariantCulture));
                    })
                    .Mappings(_ =>
                    {
                        _.FluentMappings
                        .Conventions.Setup(x => x.Add(AutoImport.Never()))
                        .AddFromAssembly(myAssembly);
                    });

                return cfg.BuildConfiguration().BuildSessionFactory();
            }
        }

        public UnitOfWork(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void BeginTransaction()
        {
            Transaction = Session.BeginTransaction();
        }

        public void Commit()
        {
            try
            {
                if (Transaction != null && Transaction.IsActive)
                    Transaction.Commit();
            }
            catch
            {
                if (Transaction != null && Transaction.IsActive)
                    Transaction.Rollback();

                throw;
            }
            finally
            {
                Session.Dispose();
            }
        }
    }
}