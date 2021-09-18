using System.Threading;
using System.Threading.Tasks;
using BookManagementSystem.Infrastructure.Domain;

namespace BookManagementSystem.Domain.Book
{
    public class BookEventHandler : 
        IEventsHandler<BookEvents.ChangeTitle, BookState>,
        IEventsHandler<BookEvents.ChangeCategory, BookState>,
        IEventsHandler<BookEvents.ChangeDescription, BookState>,
        IEventsHandler<BookEvents.AddAuthor, BookState>,
        IEventsHandler<BookEvents.RemoveAuthor, BookState>

    {
        public Task<BookState> Handle(BookEvents.ChangeTitle request, BookState state, CancellationToken cancellationToken)
        {
            var newState = state with { Title = request.Title };
            return Task.FromResult(newState);
        }

        public  Task<BookState> Handle(BookEvents.ChangeCategory request, BookState state, CancellationToken cancellationToken)
        {
            var newState = state with { CategoryId = request.CategoryId };
            return Task.FromResult(newState);
        }

        public  Task<BookState> Handle(BookEvents.ChangeDescription request, BookState state, CancellationToken cancellationToken)
        {
            var newState = state with { Description = request.Description };
            return Task.FromResult(newState);
        }

        public Task<BookState> Handle(BookEvents.AddAuthor request, BookState state, CancellationToken cancellationToken)
        {
            var newState = state with { AuthorsId = state.AuthorsId.Add(request.AuthorId) };
            return Task.FromResult(newState);
        }

        public Task<BookState> Handle(BookEvents.RemoveAuthor request, BookState state, CancellationToken cancellationToken)
        {
            var newState = state with { AuthorsId = state.AuthorsId.Remove(request.AuthorId) };
            return Task.FromResult(newState);
        }
    }
}