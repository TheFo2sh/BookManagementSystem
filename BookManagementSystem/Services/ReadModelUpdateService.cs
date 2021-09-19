﻿using System;
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
        private readonly IDatabaseRepository<BookEntity, string> _booksRepository;
        private readonly IDatabaseRepository<AuthorEntity, int> _authorsRepository;

        private readonly IUnitOfWork _unitOfWork;
        public ReadModelUpdateService(IDatabaseRepository<BookEntity, string> booksRepository, IDatabaseRepository<AuthorEntity, int> authorsRepository, IUnitOfWork unitOfWork)
        {
            _booksRepository = booksRepository;
            _unitOfWork = unitOfWork;
            _authorsRepository = authorsRepository;
        }

        public async Task Handle(BookStateChangedNotification notification, CancellationToken cancellationToken)
        {
            var bookEntity = new BookEntity()
            {
                Id = notification.State.Id,
                Title = notification.State.Title,
                Description = notification.State.Description,
                CategoryId = notification.State.CategoryId,
            };

            bookEntity.Authors = (await Task.WhenAll(
                notification.State.AuthorsId.Select(async id => await _authorsRepository.GetById(id)))).ToList();
            await _booksRepository.Upsert(notification.State.Id, bookEntity);
            await _unitOfWork.CompleteAsync();
        }
    }
}