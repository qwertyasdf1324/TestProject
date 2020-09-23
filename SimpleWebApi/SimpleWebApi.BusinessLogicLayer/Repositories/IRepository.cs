using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleWebApi.BusinessLogicLayer.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int id);
        Task<T> Create(T entity);
        void Update(T entity);
        void Delete(int id);
    }
}
