using SimpleWebApi.BusinessLogicLayer.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleWebApi.BusinessLogicLayer.Services
{
    public interface ICompanyService
    {
        Task<IEnumerable<Company>> GetAll(LimitOffset limitOffset);
        Task<Company> Get(int id);
        Task<Company> Create(Company company);
        Task<Company> Update(int id, Company company);
        Task<Company> Delete(int id);
    }
}