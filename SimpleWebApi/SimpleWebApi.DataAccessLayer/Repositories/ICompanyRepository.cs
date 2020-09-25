using SimpleWebApi.DataAccessLayer.Mappings;
using System.Threading.Tasks;

namespace SimpleWebApi.DataAccessLayer.Repositories
{
    public interface ICompanyRepository : IRepository<Company>
    {
        Task<Company> Get(string name);
    }
}
