using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Features.OwnedInstances;
using BookManagementSystem.Storage.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookManagementSystem.Storage.Database
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;


        public UnitOfWork(
            Owned<ApplicationDbContext> context,
            ILoggerFactory loggerFactory
        )
        {
            _context = context.Value;
            _logger = loggerFactory.CreateLogger("logs");

        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }

        public IWriteDatabaseRepository<T, Tkey> GetWriteRepository<T, Tkey>() where T :  BaseEntity<Tkey>
        {
            return new WriteDatabaseRepository<T, Tkey>(_context);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
