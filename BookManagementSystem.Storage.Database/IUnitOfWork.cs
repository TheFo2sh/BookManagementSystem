using System;
using System.Dynamic;
using System.Threading.Tasks;
using BookManagementSystem.Storage.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookManagementSystem.Storage.Database
{
    public interface IUnitOfWork:IDisposable
    {
        Task CompleteAsync();
        IWriteDatabaseRepository<T, Tkey> GetWriteRepository<T, Tkey>() where T : BaseEntity<Tkey>;
    }
}