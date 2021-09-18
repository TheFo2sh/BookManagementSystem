using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookManagementSystem.Infrastructure.Domain
{
    public interface IEventsRepository
    {
        IAsyncEnumerable<Task<object>> GetEvents(string aggregateType, string aggregateId);
        Task<long> CommitAsync(string aggregateType, string aggregateId, object evt);
    }
}