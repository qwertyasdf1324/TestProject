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

        public async Task<Company> Get(string name) => await UnitOfWork.Session.Query<Company>().FirstAsync(_ => _.Name.Equals(name));

        public async Task<Company> Create(Company company)
        {
            UnitOfWork.BeginTransaction();

            var allCertificates = await UnitOfWork.Session.Query<Certificate>().ToListAsync();
            var certificatesToAdd = company.Certificates.Select(_ => _.Number).Select(async _ => await UnitOfWork.Session.Query<Certificate>().FirstAsync(__ => __.Number == _)).ToList();

            company.Certificates.Clear();

            await UnitOfWork.Session.SaveAsync(company);

            UnitOfWork.Commit();

            var savedCompany = await Get(company.Name);

            certificatesToAdd.ForEach(async _ =>
            {
                UnitOfWork.BeginTransaction();

                await UnitOfWork.Session.SaveAsync(new CompanyCertificate()
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
    }
}