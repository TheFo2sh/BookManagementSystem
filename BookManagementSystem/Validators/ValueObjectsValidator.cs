using System.Threading.Tasks;
using BookManagementSystem.Domain.ValueObjects;
using BookManagementSystem.Storage.Database;
using BookManagementSystem.Storage.Database.Entities;

namespace BookManagementSystem.Validators
{
    public class ValueObjectsValidator: ICategoryValidator, IAuthorValidator
    {
        private readonly IReadDatabaseRepository<CategoryEntity, int> _categoryRepository;
        private readonly IReadDatabaseRepository<AuthorEntity, int> _authorRepository;

        public ValueObjectsValidator(IReadDatabaseRepository<CategoryEntity, int> categoryRepository, IReadDatabaseRepository<AuthorEntity, int> authorRepository)
        {
            _categoryRepository = categoryRepository;
            _authorRepository = authorRepository;
        }

        public async Task<bool> Validate(int value, Category owner)
        {
            return await _categoryRepository.GetById(value) != null;
        }

        public async Task<bool> Validate(int value, Author owner)
        {
            return await _authorRepository.GetById(value) != null;

        }
    }
}