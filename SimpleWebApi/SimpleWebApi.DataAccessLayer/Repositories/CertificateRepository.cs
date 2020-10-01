using NHibernate;
using NHibernate.Linq;
using SimpleWebApi.DataAccessLayer.Helpers;
using SimpleWebApi.DataAccessLayer.Mappings;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace SimpleWebApi.DataAccessLayer.Repositories
{
    public class CertificateRepository : ICertificateRepository
    {
        public ISession Session { get; set; }

        public CertificateRepository(ISession session)
        {
            Session = session;
        }

        public async Task<IEnumerable<Certificate>> GetAll() => await Session.Query<Certificate>().ToListAsync();

        public Task<Certificate> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Certificate> Create(Certificate entity)
        {
            throw new NotImplementedException();
        }

        public async Task<Certificate> GetByNumber(int number) => await Session.Query<Certificate>().FirstAsync(_ => _.Number == number);

        public Task<Certificate> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Certificate> Update(Certificate entity)
        {
            throw new NotImplementedException();
        }

        public Task<Certificate> Update(int id, Certificate entity)
        {
            throw new NotImplementedException();
        }
    }
}