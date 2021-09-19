using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BookManagementSystem.Domain.Commands;
using BookManagementSystem.Infrastructure.Domain;
using BookManagementSystem.Storage.Database;
using BookManagementSystem.Storage.Database.Entities;
using MediatR;

namespace BookManagementSystem.Validators
{
    public class UpdateBookCommandsValidator:ICommandValidator<ChangeCategoryCommand>, ICommandValidator<AddAuthorCommand>
    {
        private readonly IDatabaseRepository<CategoryEntity, int> _categoryRepository;
        private readonly IDatabaseRepository<AuthorEntity, int> _authorRepository;

        public UpdateBookCommandsValidator(IDatabaseRepository<CategoryEntity, int> categoryRepository, IDatabaseRepository<AuthorEntity, int> authorRepository)
        {
            _categoryRepository = categoryRepository;
            _authorRepository = authorRepository;
        }


        public async Task<bool> Handle(ChangeCategoryCommand request, CancellationToken cancellationToken, RequestHandlerDelegate<bool> next)
        {
            var category = await _categoryRepository.GetById(request.Category);
            if (category == null)
                return false;
            return await next();
        }

        public async Task<bool> Handle(AddAuthorCommand request, CancellationToken cancellationToken, RequestHandlerDelegate<bool> next)
        {
            var author = await _authorRepository.GetById(request.Author);
            if (author == null)
                return false;

            return await next();

        }
    }
}
