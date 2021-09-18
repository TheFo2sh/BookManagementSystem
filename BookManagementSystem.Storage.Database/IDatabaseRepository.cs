using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookManagementSystem.Storage.Database
{
    public interface IDatabaseRepository<T, TKey> where T : class
    {
        Task<IEnumerable<T>> All();
        Task<T> GetById(TKey id);
        Task<bool> Add(T entity);
        Task<bool> Delete(TKey id);
        Task<bool> Upsert(T entity);
    }
}