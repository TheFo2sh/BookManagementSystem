using System.Threading;
using System.Threading.Tasks;

namespace BookManagementSystem.Infrastructure.Domain
{
    public interface IEventsHandler<in TEvent, TState>
    {
        Task<TState> Handle(TEvent request, TState state, CancellationToken cancellationToken);
    }
}