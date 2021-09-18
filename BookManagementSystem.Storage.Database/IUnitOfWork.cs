using System.Threading.Tasks;

namespace BookManagementSystem.Storage.Database
{
    public interface IUnitOfWork
    {
        Task CompleteAsync();
    }
}