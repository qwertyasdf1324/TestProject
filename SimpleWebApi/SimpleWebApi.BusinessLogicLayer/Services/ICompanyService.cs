using SimpleWebApi.BusinessLogicLayer.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleWebApi.BusinessLogicLayer.Services
{
    public interface ICompanyService
    {
        Task<IEnumerable<Company>> GetAll();
        Task<Company> GetById(int id);
        //Task<Company> Create(Company company);
    }
}