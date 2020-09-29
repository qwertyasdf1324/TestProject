using NHibernate.Linq;
using SimpleWebApi.DataAccessLayer.Helpers;
using SimpleWebApi.DataAccessLayer.Mappings;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleWebApi.DataAccessLayer.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        public UnitOfWork UnitOfWork { get; set; }

        public CompanyRepository(IUnitOfWork unitOfWork)
        {
            UnitOfWork = (UnitOfWork)unitOfWork;
        }

        public async Task<IEnumerable<Company>> GetAll() => await UnitOfWork.Session.Query<Company>().ToListAsync();

        public async Task<Company> Get(int id) => await UnitOfWork.Session.GetAsync<Company>(id);

        public async Task<Company> Create(Company company)
        {
            UnitOfWork.BeginTransaction();

            var allCertificates = await UnitOfWork.Session.Query<Certificate>().ToListAsync();
            var certificatesToAdd = company.Certificates.Select(_ => _.Number).Select(async _ => await UnitOfWork.Session.Query<Certificate>().FirstAsync(__ => __.Number == _)).ToList();

            company.Certificates.Clear();

            await UnitOfWork.Session.SaveAsync(company);

            UnitOfWork.Commit();

            var savedCompany = await Get(company.Id);

            certificatesToAdd.ForEach(async _ =>
            {
                UnitOfWork.BeginTransaction();

                await UnitOfWork.Session.SaveAsync(new CompanyCertificate()
                {
                    CompanyId = savedCompany.Id,
                    CertificateId = _.Result.Id
                });

                UnitOfWork.Commit(); ;
            });

            company.Certificates = new List<Certificate>();

            certificatesToAdd.ForEach(_ => company.Certificates.Add(_.Result));

            return company;
        }

        public Task<Company> Delete(int id)
        {
            //UnitOfWork.Session.FlushMode = NHibernate.FlushMode.Commit;

            var companyCertificateAssociations = UnitOfWork.Session.Query<CompanyCertificate>().Select(_ => _).Where(_ => _.CompanyId == id).ToList();

            UnitOfWork.BeginTransaction();

            companyCertificateAssociations.ForEach(_ =>
            {
                //UnitOfWork.Session.LoadAsync<CompanyCertificate>(_.Id);
                UnitOfWork.Session.Delete(_);
            });

            //UnitOfWork.Commit();
            //UnitOfWork.Session.Flush();

            //UnitOfWork.BeginTransaction();
            //await UnitOfWork.Session.LoadAsync<Company>(companyToDelete.Id);
            var companyToDelete = UnitOfWork.Session.Query<Company>().First(_ => _.Id == id);
            try
            {
                UnitOfWork.Session.Delete(companyToDelete);
            }
            catch (System.Exception message)
            {

                throw;
            }

            UnitOfWork.Commit();

            return null;
        }

        public async Task<Company> Update(int id, Company company)
        {
            UnitOfWork.BeginTransaction();

            var allCertificates = await UnitOfWork.Session.Query<Certificate>().ToListAsync();
            var certificatesToAdd = company.Certificates.Select(_ => _.Number).Select(async _ => await UnitOfWork.Session.Query<Certificate>().FirstAsync(__ => __.Number == _)).ToList();

            company.Certificates.Clear();

            await UnitOfWork.Session.UpdateAsync(company);

            UnitOfWork.Commit();

            var savedCompany = await Get(id);

            certificatesToAdd.ForEach(async _ =>
            {
                UnitOfWork.BeginTransaction();

                await UnitOfWork.Session.SaveOrUpdateAsync(new CompanyCertificate()
                {
                    CompanyId = savedCompany.Id,
                    CertificateId = _.Result.Id
                });

                UnitOfWork.Commit();
            });

            company.Certificates = new List<Certificate>();

            certificatesToAdd.ForEach(_ => company.Certificates.Add(_.Result));

            return company;
        }

        public async Task<IEnumerable<Company>> GetAll(int limit, int offset) => await UnitOfWork.Session.Query<Company>().Skip(offset).Take(limit).ToListAsync();
    }
}