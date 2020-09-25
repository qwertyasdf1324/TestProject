using SimpleWebApi.BusinessLogicLayer.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleWebApi.BusinessLogicLayer.Services
{
    public interface ICompanyService
    {
        Task<IEnumerable<Company>> GetAll();
        Task<Company> Get(int id);
        Task<Company> Get(string name);
        Task<Company> Create(Company company);
    }
}