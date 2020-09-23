using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NHibernate;
using NHibernate.Linq;
using SimpleWebApi.Models;

namespace SimpleWebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        public ISession Session { get; set; }

        public CompaniesController(ISession session) 
        {
            Session = session;
        }

        // GET: /Companies
        [HttpGet]

        public async Task<ActionResult<IEnumerable<Company>>> GetAll() => await Session.Query<Company>().ToListAsync();

        // GET /Companies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Company>> Get(int id)
        {
            var companyToFind = await Session.Query<Company>().FirstAsync(_ => _.Id == id);
            

            //if (companyToFind) 
            //{
            //    return NotFound();
            //}

            return companyToFind;
        }

        // POST /Companies
        [HttpPost]
        public async Task<ActionResult<Company>> Post(Company company)
        {
            var certificatesToAdd = new List<Certificate>();

            using (ITransaction transaction = Session.BeginTransaction())
            {
                var allCertificates = await Session.Query<Certificate>().ToListAsync();

                foreach (var certificate in company.Certificates)
                {
                    var foundCertificate = allCertificates.First(_ => _.Number == certificate.Number);

                    certificatesToAdd.Add(foundCertificate);
                }

                company.Certificates = null;
                company.Certificates = new List<Certificate>();

                await Session.SaveAsync(company);

                await transaction.CommitAsync();

                var companies2 = await GetAll();
                var ids2 = companies2.Value.Select(_ => _.Id).ToList();
                ids2.Sort();

                foreach (var certificate in certificatesToAdd)
                {
                    using (ITransaction transactionw = Session.BeginTransaction())
                    {
                        Session.Save(new CompanyCertificate()
                        {
                            CompanyId = ids2.LastOrDefault(),
                            CertificateId = certificate.Id
                        });

                        transactionw.Commit();
                    }
                }
            }

            var companies = await GetAll();
            var ids = companies.Value.Select(_ => _.Id).ToList();
            ids.Sort();

            company.Certificates = new List<Certificate>();

            foreach (var item in certificatesToAdd)
            {
                company.Certificates.Add(item);
            }

            return CreatedAtAction("Get", new { id = ids.LastOrDefault() }, company);
        }
    }
}
