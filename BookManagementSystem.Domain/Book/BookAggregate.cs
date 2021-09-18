using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using BookManagementSystem.Infrastructure.Domain;
using MediatR;

namespace BookManagementSystem.Domain.Book
{
    public class BookAggregate : BaseDomainObject<BookEventHandler, BookState>
    {
        public BookAggregate(string bookId, IEventsRepository eventStoreService,IMediator mediator)
            : base(bookId, new BookState(bookId, AuthorsId: ImmutableList<int>.Empty), eventStoreService, mediator)
        {
        }

        public async Task ChangeTitle(string title)
        {
            var testEvent = new BookEvents.ChangeTitle(title, DateTime.Now);
            await ApplyAndCommitAsync(testEvent);
        }
        public async Task ChangeDescription(string description)
        {
            var testEvent = new BookEvents.ChangeDescription(description, DateTime.Now);
            await ApplyAndCommitAsync(testEvent);
        }
        public async Task ChangeCategory(int category)
        {
            var testEvent = new BookEvents.ChangeCategory(category, DateTime.Now);
            await ApplyAndCommitAsync(testEvent);
        }
        public async Task AddAuthor(int authorId)
        {
            var testEvent = new BookEvents.AddAuthor(authorId, DateTime.Now);
            await ApplyAndCommitAsync(testEvent);
        }
        public async Task RemoveAuthor(int authorId)
        {
            var testEvent = new BookEvents.RemoveAuthor(authorId, DateTime.Now);
            await ApplyAndCommitAsync(testEvent);
        }
        protected override async void OnEventsCommitted(BookState state, long position)
        {
            await Mediator.Publish(new BookStateChangedNotification(state));
        }

        public BookState GetState() => GetCurrentState();
    }
}