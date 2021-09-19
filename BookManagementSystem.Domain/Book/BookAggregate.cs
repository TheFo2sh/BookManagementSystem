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

        public async Task ChangeTitle(string title,EventsTransaction transaction=null)
        {
            var testEvent = new BookEvents.ChangeTitle(title, DateTime.Now);
            await Apply(testEvent, transaction);
        }
        public async Task ChangeDescription(string description, EventsTransaction transaction = null)
        {
            var testEvent = new BookEvents.ChangeDescription(description, DateTime.Now);
            await Apply(testEvent, transaction);
        }
        public async Task ChangeCategory(int category, EventsTransaction transaction = null)
        {
            var testEvent = new BookEvents.ChangeCategory(category, DateTime.Now);
            await Apply(testEvent, transaction);
        }
        public async Task AddAuthor(int authorId, EventsTransaction transaction = null)
        {
            var testEvent = new BookEvents.AddAuthor(authorId, DateTime.Now);
            await Apply(testEvent, transaction);
        }

        public async Task RemoveAuthor(int authorId, EventsTransaction transaction = null)
        {
            var testEvent = new BookEvents.RemoveAuthor(authorId, DateTime.Now);
            await Apply(testEvent, transaction);
        }
        protected override async void OnEventsCommitted(BookState state, long position)
        {
            await Mediator.Publish(new BookStateChangedNotification(state));
        }

        private async Task Apply<T>( T testEvent, EventsTransaction transaction)
        {
            if (transaction == null)
                await ApplyAndCommitAsync(testEvent);
            else
            {
                await Apply(testEvent);
                transaction.Add(testEvent);
            }
        }
        public BookState GetState() => GetCurrentState();
    }
}