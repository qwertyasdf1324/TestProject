using SimpleWebApi.DataAccessLayer.Mappings;
using System.Threading.Tasks;

namespace SimpleWebApi.DataAccessLayer.Repositories
{
    public interface ICertificateRepository : IRepository<Certificate>
    {
        Task<Certificate> GetByNumber(int number);
    }
}