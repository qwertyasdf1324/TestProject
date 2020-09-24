using Microsoft.Extensions.DependencyInjection;
using SimpleWebApi.DataAccessLayer.Helpers;
using SimpleWebApi.DataAccessLayer.Mappings;
using SimpleWebApi.DataAccessLayer.Repositories;

namespace SimpleWebApi.BusinessLogicLayer.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void MapRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IRepository<Company>, Repository<Company>>();
            services.AddSingleton<IUnitOfWork, UnitOfWork>();
        }
    }
}