using AutoMapper;
using DAL = SimpleWebApi.DataAccessLayer.Mappings;
using BLL = SimpleWebApi.BusinessLogicLayer.DTOs;
using SimpleWebApi.DataAccessLayer.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleWebApi.BusinessLogicLayer.Services
{
    public class CompanyService : ICompanyService
    {
        #region Configuration

        Mapper companyServiceObjMapper => new Mapper(new MapperConfiguration(_ =>
        {
            _.CreateMap<DAL.Company, BLL.Company>()
            .ReverseMap();
            _.CreateMap<DAL.Address, BLL.Address>()
            .ReverseMap();
            _.CreateMap<DAL.Certificate, BLL.Certificate>()
            .ReverseMap();
        }));

        #endregion

        private IRepository<DAL.Company> companyRepository;

        public CompanyService(IRepository<DAL.Company> companyRepository)
        {
            this.companyRepository = companyRepository;
        }

        public async Task<IEnumerable<BLL.Company>> GetAll() => companyServiceObjMapper.Map<IEnumerable<DAL.Company>, IEnumerable<BLL.Company>>(await companyRepository.GetAll());

        public async Task<BLL.Company> GetById(int id) => companyServiceObjMapper.Map<DAL.Company, BLL.Company>(await companyRepository.GetById(id));
        //
        //public async Task<BLL.Company> Create(BLL.Company company) => await companyRepository.Create(company);
    }
}