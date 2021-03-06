using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookManagementSystem.Storage.Database
{
    public interface IReadDatabaseRepository<T, TKey> where T : class
    {
        IQueryable<T> All();
        Task<T> GetById(TKey id);
        Task<int> Count();

    }
}