using System.Threading;
using System.Threading.Tasks;
using BookManagementSystem.Infrastructure.Domain;

namespace BookManagementSystem.Domain.Book
{
    public class BookEventHandler : 
        IEventsHandler<BookEvents.ChangeTitle, BookState>,
        IEventsHandler<BookEvents.AddAuthor, BookState>
    {
        public Task<BookState> Handle(BookEvents.ChangeTitle request, BookState state, CancellationToken cancellationToken)
        {
            var newState = state with { Title = request.Title };
            return Task.FromResult(newState);
        }

        public Task<BookState> Handle(BookEvents.AddAuthor request, BookState state, CancellationToken cancellationToken)
        {
            var newState = state with { AuthorsId = state.AuthorsId.Add(request.Author) };
            return Task.FromResult(newState);
        }
    }
}