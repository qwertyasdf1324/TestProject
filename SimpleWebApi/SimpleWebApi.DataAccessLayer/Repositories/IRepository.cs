using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleWebApi.DataAccessLayer.Repositories
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int id);
        Task<T> Create(T entity);
    }
}