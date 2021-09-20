using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Storage.Database;
using BookManagementSystem.Storage.Database.Entities;
using BookManagementSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookManagementSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataController : ControllerBase
    {
        private readonly IReadDatabaseRepository<CategoryEntity, int> _categoryRepository;
        private readonly IReadDatabaseRepository<AuthorEntity, int> _authorRepository;

        public DataController(IReadDatabaseRepository<CategoryEntity, int> categoryRepository, IReadDatabaseRepository<AuthorEntity, int> authorRepository)
        {
            _categoryRepository = categoryRepository;
            _authorRepository = authorRepository;
        }

        [HttpGet("categories")]
        public async Task<IEnumerable<CategoryViewModel>> GetCategories()
        {
           return await _categoryRepository.All().Select(cat => new CategoryViewModel() { Id = cat.Id, Category = cat.Name })
                .ToListAsync();
        }

        [HttpGet("authors")]
        public async Task<IEnumerable<AuthorViewModel>> GetAuthors()
        {
            return await _authorRepository.All().Select(cat => new AuthorViewModel() { Id = cat.Id, Author = cat.Name })
                .ToListAsync();

        }
    }
}