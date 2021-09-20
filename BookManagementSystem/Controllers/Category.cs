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
        private readonly IDatabaseRepository<CategoryEntity, int> _categoryRepository;
        private readonly IDatabaseRepository<AuthorEntity, int> _authorRepository;

        public DataController(IDatabaseRepository<CategoryEntity, int> categoryRepository, IDatabaseRepository<AuthorEntity, int> authorRepository)
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