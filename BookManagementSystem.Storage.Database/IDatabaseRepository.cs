using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookManagementSystem.Storage.Database
{
    public interface IDatabaseRepository<T, TKey> where T : class
    {
        IQueryable<T> All();
        Task<T> GetById(TKey id);
        Task<bool> Add(T entity);
        Task<bool> Delete(TKey id);
        Task<bool> Upsert(string id, T entity);
    }
}