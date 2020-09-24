
namespace SimpleWebApi.DataAccessLayer.Helpers
{
    public interface IUnitOfWork
    {
        void BeginTransaction();
        void Commit();
    }
}