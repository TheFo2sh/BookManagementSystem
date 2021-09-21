using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BookManagementSystem.Infrastructure.Domain;

namespace BookManagementSystem.Domain.Book
{
    public class BookEventHandler : 
        IEventsHandler<BookEvents.TitleChanged, BookState>,
        IEventsHandler<BookEvents.CategoryChanged, BookState>,
        IEventsHandler<BookEvents.DescriptionChanged, BookState>,
        IEventsHandler<BookEvents.AuthorAdded, BookState>,
        IEventsHandler<BookEvents.AuthorRemoved, BookState>

    {
        public Task<BookState> Handle(BookEvents.TitleChanged request, BookState state, CancellationToken cancellationToken)
        {
            var newState = state with { Title = request.Title };
            return Task.FromResult(newState);
        }

        public  Task<BookState> Handle(BookEvents.CategoryChanged request, BookState state, CancellationToken cancellationToken)
        {
            var newState = state with { CategoryId = request.CategoryId };
            return Task.FromResult(newState);
        }

        public  Task<BookState> Handle(BookEvents.DescriptionChanged request, BookState state, CancellationToken cancellationToken)
        {
            var newState = state with { Description = request.Description };
            return Task.FromResult(newState);
        }

        public Task<BookState> Handle(BookEvents.AuthorAdded request, BookState state, CancellationToken cancellationToken)
        {
            if (state.AuthorsId.Contains(request.AuthorId))
            {
                return Task.FromResult(state);
            }
            var newState = state with { AuthorsId = state.AuthorsId.Add(request.AuthorId) };
            return Task.FromResult(newState);
        }

        public Task<BookState> Handle(BookEvents.AuthorRemoved request, BookState state, CancellationToken cancellationToken)
        {
            if (!state.AuthorsId.Contains(request.AuthorId))
            {
                return Task.FromResult(state);
            }
            var newState = state with { AuthorsId = state.AuthorsId.Remove(request.AuthorId) };
            return Task.FromResult(newState);
        }
    }
}