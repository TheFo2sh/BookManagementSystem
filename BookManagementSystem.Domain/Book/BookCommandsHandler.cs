using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BookManagementSystem.Domain.Book;
using BookManagementSystem.Domain.Commands;
using BookManagementSystem.Infrastructure.Domain;
using Dasync.Collections;
using MediatR;

namespace BookManagementSystem.Domain.Book
{
    public class BookCommandsHandler : ICommandHandler<ChangeTitleCommand>,
        ICommandHandler<ChangeCategoryCommand>,
        ICommandHandler<ChangeDescriptionCommand>,
        ICommandHandler<AddAuthorCommand>,
        ICommandHandler<RemoveAuthorCommand>,
        ICommandHandler<CreateBookCommand>
    {
        private readonly DomainObjectRepository<BookAggregate, BookEventHandler, BookState> _repository;

        public BookCommandsHandler(DomainObjectRepository<BookAggregate, BookEventHandler, BookState> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(ChangeTitleCommand request, CancellationToken cancellationToken)
        {
            var aggregate = await _repository.GetAsync(request.AggregateId);
            await aggregate.ChangeTitle(request.Title);
            return true;
        }

        public async Task<bool> Handle(ChangeCategoryCommand request, CancellationToken cancellationToken)
        {
            var aggregate = await _repository.GetAsync(request.AggregateId);
            await aggregate.ChangeCategory(request.Category);
            return true;
        }

        public async Task<bool> Handle(ChangeDescriptionCommand request, CancellationToken cancellationToken)
        {
            var aggregate = await _repository.GetAsync(request.AggregateId);
            await aggregate.ChangeDescription(request.Description);
            return true;
        }

        public async Task<bool> Handle(AddAuthorCommand request, CancellationToken cancellationToken)
        {
            var aggregate = await _repository.GetAsync(request.AggregateId);
            await aggregate.AddAuthor(request.Author);
            return true;
        }

        public async Task<bool> Handle(RemoveAuthorCommand request, CancellationToken cancellationToken)
        {
            var aggregate = await _repository.GetAsync(request.AggregateId);
            await aggregate.RemoveAuthor(request.Author);
            return true;
        }

        public async Task<bool> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            var aggregate = await _repository.GetAsync(request.AggregateId);
            var transaction = aggregate.GetTransaction();
                await aggregate.ChangeTitle(request.Title, transaction);
                await aggregate.ChangeDescription(request.Description, transaction);
                await aggregate.ChangeCategory(request.CategoryId, transaction);

                await request.AuthorsId.ParallelForEachAsync(id => aggregate.AddAuthor(id, transaction), cancellationToken);
                await aggregate.CommitAsync(transaction);
            return true;
        }
    }
}
