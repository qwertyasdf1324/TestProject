using NHibernate;
using NHibernate.Linq;
using SimpleWebApi.DataAccessLayer.Helpers;
using SimpleWebApi.DataAccessLayer.Mappings;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace SimpleWebApi.DataAccessLayer.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        public ISession Session { get; set; }

        public CompanyRepository(ISession session)
        {
            Session = session;
        }

        public async Task<IEnumerable<Company>> GetAll() => await Session.Query<Company>().ToListAsync();

        public async Task<Company> Get(int id) => await Session.GetAsync<Company>(id);

        public async Task<Company> Create(Company company)
        {
            List<Certificate> certificatesToAdd = new List<Certificate>();

            using (var transaction = Session.BeginTransaction())
            {
                var allCertificates = await Session.Query<Certificate>().ToListAsync();
                certificatesToAdd = company.Certificates.Select(_ => _.Number).Select(_ => Session.Query<Certificate>().First(__ => __.Number == _)).ToList();

                company.Certificates.Clear();

                await Session.SaveAsync(company);

                try
                {
                    if (transaction != null && transaction.IsActive)
                        transaction.Commit();
                }
                catch
                {
                    if (transaction != null && transaction.IsActive)
                        transaction.Rollback();

                    throw;
                }
            }

            var savedCompany = await Get(company.Id);

            certificatesToAdd.ForEach(_ =>
            {
                using (var transaction = Session.BeginTransaction())
                {
                    Session.Save(new CompanyCertificate()
                    {
                        CompanyId = savedCompany.Id,
                        CertificateId = _.Id
                    });

                    try
                    {
                        if (transaction != null && transaction.IsActive)
                            transaction.Commit();
                    }
                    catch
                    {
                        if (transaction != null && transaction.IsActive)
                            transaction.Rollback();
                        throw;
                    }
                }
            });

            company.Certificates = new List<Certificate>();

            certificatesToAdd.ForEach(_ => company.Certificates.Add(_));

            return company;
        }

        public async Task<Company> Delete(int id)
        {
            Company companyToDelete;

            using (var transaction = Session.BeginTransaction())
            {
                var companyCertificateAssociations = await Session.Query<CompanyCertificate>().Select(_ => _).Where(_ => _.CompanyId == id).ToListAsync();

                companyCertificateAssociations.ForEach(async _ => await Session.DeleteAsync(_));

                await transaction.CommitAsync();
            }

            using (var transaction = Session.BeginTransaction())
            {
                companyToDelete = await Session.GetAsync<Company>(id);

                if (companyToDelete == null)
                {
                    return null;
                }

                await Session.DeleteAsync(companyToDelete);

                await transaction.CommitAsync();
            }

            return companyToDelete;
        }

        public async Task<Company> Update(int id, Company company)
        {
            List<Certificate> certificatesToAdd = new List<Certificate>();

            using (var transaction = Session.BeginTransaction())
            {
                var allCertificates = await Session.Query<Certificate>().ToListAsync();
                certificatesToAdd = company.Certificates.Select(_ => _.Number).Select(_ => Session.Query<Certificate>().First(__ => __.Number == _)).ToList();

                company.Certificates.Clear();

                await Session.UpdateAsync(company);

                try
                {
                    if (transaction != null && transaction.IsActive)
                        transaction.Commit();
                }
                catch
                {
                    if (transaction != null && transaction.IsActive)
                        transaction.Rollback();

                    throw;
                }
            }

            var savedCompany = await Get(company.Id);

            certificatesToAdd.ForEach(_ =>
            {
                using (var transaction = Session.BeginTransaction())
                {
                    Session.SaveOrUpdate(new CompanyCertificate()
                    {
                        CompanyId = savedCompany.Id,
                        CertificateId = _.Id
                    });

                    try
                    {
                        if (transaction != null && transaction.IsActive)
                            transaction.Commit();
                    }
                    catch
                    {
                        if (transaction != null && transaction.IsActive)
                            transaction.Rollback();
                        throw;
                    }
                }
            });

            company.Certificates = new List<Certificate>();

            certificatesToAdd.ForEach(_ => company.Certificates.Add(_));

            return company;
        }

        public async Task<IEnumerable<Company>> GetAll(int limit, int offset) => await Session.Query<Company>().Skip(offset).Take(limit).ToListAsync();
    }
}