using Microsoft.Extensions.DependencyInjection;
using SimpleWebApi.DataAccessLayer.Helpers;
using SimpleWebApi.DataAccessLayer.Repositories;

namespace SimpleWebApi.BusinessLogicLayer.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void MapRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICertificateRepository, CertificateRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddSingleton<IUnitOfWork, UnitOfWork>();
        }
    }
}