using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using BookManagementSystem.Domain.ValueObjects;
using BookManagementSystem.Infrastructure.Domain;
using MediatR;
using ValueOf;

namespace BookManagementSystem.Domain.Book
{
    public class BookAggregate : BaseDomainObject<BookEventHandler, BookState>
    {
        private readonly ICategoryValidator _categoryValidator;
        private readonly IAuthorValidator _authorValidator;
        public BookAggregate(string bookId, IEventsRepository eventStoreService,IMediator mediator, ICategoryValidator categoryValidator, IAuthorValidator authorValidator)
            : base(bookId, new BookState(bookId, AuthorsId: ImmutableList<int>.Empty), eventStoreService, mediator)
        {
            _categoryValidator = categoryValidator;
            _authorValidator = authorValidator;
        }

        public async Task ChangeTitle(string title,EventsTransaction transaction=null)
        {
            var testEvent = new BookEvents.TitleChanged(title, DateTime.Now);
            await Apply(testEvent, transaction);
        }
        public async Task ChangeDescription(string description, EventsTransaction transaction = null)
        {
            var testEvent = new BookEvents.DescriptionChanged(description, DateTime.Now);
            await Apply(testEvent, transaction);
        }
        public async Task ChangeCategory(int categoryId, EventsTransaction transaction = null)
        {
            var category = await ValueOf.Factory.CreateFromAsync<Category>(categoryId);
            var testEvent = new BookEvents.CategoryChanged(category, DateTime.Now);
            await Apply(testEvent, transaction);
        }
        public async Task AddAuthor(int authorId, EventsTransaction transaction = null)
        {
            var testEvent = new BookEvents.AuthorAdded(authorId, DateTime.Now);
            await Apply(testEvent, transaction);
        }

        public async Task RemoveAuthor(int authorId, EventsTransaction transaction = null)
        {
            var testEvent = new BookEvents.AuthorRemoved(authorId, DateTime.Now);
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