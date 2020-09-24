using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using Microsoft.Extensions.Configuration;
using NHibernate;
using System;
using System.Globalization;
using System.Linq;
using Environment = NHibernate.Cfg.Environment;

namespace SimpleWebApi.DataAccessLayer.Helpers
{
    public class UnitOfWork : IUnitOfWork
    {
        IConfiguration configuration;

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
        public ITransaction _transaction;

        public ISession Session => SessionFactory.OpenSession();

        public UnitOfWork(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void BeginTransaction()
        {
            _transaction = Session.BeginTransaction();
        }

        public void Commit()
        {
            try
            {
                if (_transaction != null && _transaction.IsActive)
                    _transaction.Commit();
            }
            catch
            {
                if (_transaction != null && _transaction.IsActive)
                    _transaction.Rollback();

                throw;
            }
            finally
            {
                Session.Dispose();
            }
        }
    }
}