using SimpleWebApi.DataAccessLayer.Mappings;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleWebApi.DataAccessLayer.Repositories
{
    public interface ICompanyRepository : IRepository<Company>
    {
        Task<IEnumerable<Company>> GetAll(int limit, int offset);
    }
}