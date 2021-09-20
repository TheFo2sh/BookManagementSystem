using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BookManagementSystem.Domain.Book;
using BookManagementSystem.Storage.Database;
using BookManagementSystem.Storage.Database.Entities;
using MediatR;

namespace BookManagementSystem.Services
{
    public class ReadModelUpdateService:INotificationHandler<BookStateChangedNotification>
    {
        private readonly IWriteDatabaseRepository<BookEntity, string> _booksRepository;
        private readonly IWriteDatabaseRepository<AuthorEntity, int> _authorsRepository;
        private readonly IWriteDatabaseRepository<CategoryEntity, int> _categoryRepository;

        private readonly IUnitOfWork _unitOfWork;
        public ReadModelUpdateService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            _booksRepository = _unitOfWork.GetWriteRepository<BookEntity, string>();
            _categoryRepository = _unitOfWork.GetWriteRepository<CategoryEntity, int>(); 
            _authorsRepository = _unitOfWork.GetWriteRepository<AuthorEntity, int>(); 
        }

        public async Task Handle(BookStateChangedNotification notification, CancellationToken cancellationToken)
        {
            try
            {
                var bookEntityAuthors =
                    (await Task.WhenAll(
                        notification.State.AuthorsId.Select(async id => await _authorsRepository.GetById(id)))).ToList();

                var oldBook = await _booksRepository.GetById(notification.State.Id);
                if (oldBook != null)
                {
                    await UpdateBook(notification, oldBook);
                }
                else
                {
                    await AddBook(notification, bookEntityAuthors);
                }

                await _unitOfWork.CompleteAsync();
            }
            finally
            {
                _unitOfWork.Dispose();
            }
        }

        private async Task AddBook(BookStateChangedNotification notification, List<AuthorEntity> bookEntityAuthors)
        {
            var bookEntity = new BookEntity()
            {
                Id = notification.State.Id,
                Title = notification.State.Title,
                Description = notification.State.Description,
                CategoryId = notification.State.CategoryId,
            };

            if (notification.State.CategoryId.HasValue)
                bookEntity.Category =
                    await _categoryRepository.GetById(notification.State.CategoryId.GetValueOrDefault());

            bookEntity.Authors = bookEntityAuthors;

            await _booksRepository.Add(bookEntity);
        }

        private async Task UpdateBook(BookStateChangedNotification notification, BookEntity oldBook)
        {
            oldBook.Title = notification.State.Title;
            oldBook.Description = notification.State.Description;
            oldBook.CategoryId = notification.State.CategoryId;

            if (notification.State.CategoryId.HasValue)
                oldBook.Category = await _categoryRepository.GetById(notification.State.CategoryId.GetValueOrDefault());

            foreach (var aurhorId in notification.State.AuthorsId)
            {
                if (!oldBook.Authors.Any(author => author.Id == aurhorId))
                    oldBook.Authors.Add(new AuthorEntity() { Id = aurhorId });
            }

            _booksRepository.Update(oldBook);
        }

      
    }
}
