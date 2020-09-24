using NHibernate.Linq;
using SimpleWebApi.DataAccessLayer.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleWebApi.DataAccessLayer.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        public UnitOfWork UnitOfWork { get; set; }

        public Repository(IUnitOfWork unitOfWork)
        {
            UnitOfWork = (UnitOfWork)unitOfWork;
        }

        public async Task<IEnumerable<T>> GetAll() => await UnitOfWork.Session.Query<T>().ToListAsync();

        public async Task<T> GetById(int id) => await UnitOfWork.Session.GetAsync<T>(id);

        public Task<T> Create(T entity)
        {
            throw new NotImplementedException();
        }
    }
}