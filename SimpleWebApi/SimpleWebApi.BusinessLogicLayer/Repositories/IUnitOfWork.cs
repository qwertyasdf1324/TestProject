
namespace SimpleWebApi.BusinessLogicLayer.Repositories
{
    public interface IUnitOfWork
    {
        void BeginTransaction();
        void Commit();
    }
}
