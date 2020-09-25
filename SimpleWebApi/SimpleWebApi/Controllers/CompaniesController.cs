using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SimpleWebApi.BusinessLogicLayer.DTOs;
using SimpleWebApi.BusinessLogicLayer.Services;

namespace SimpleWebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        public ICompanyService companyService { get; set; }

        public CompaniesController(ICompanyService companyService)
        {
            this.companyService = companyService;
        }

        // GET: /Companies
        [HttpGet]

        public async Task<ActionResult<IEnumerable<Company>>> GetAll() => Ok(await companyService.GetAll());


        // GET /Companies/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Company>> Get(int id)
        {
            var companyToFind = await companyService.Get(id);

            if (companyToFind == null)
            {
                return NotFound(new
                {
                    message = $"There's nothing found by this id:{id} - it doesn't exist, try another one."
                });
            }

            return companyToFind;
        }

        // POST /Companies
        [HttpPost]
        public async Task<ActionResult<Company>> Post(Company company)
        {
            var savedCompany = await companyService.Create(company);

            return CreatedAtAction("Get", new { id = savedCompany.Id }, savedCompany);
        }
    }
}
