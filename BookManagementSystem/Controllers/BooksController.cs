using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Domain.Book;
using BookManagementSystem.Domain.Commands;
using BookManagementSystem.Infrastructure.Domain;
using BookManagementSystem.Requests;
using BookManagementSystem.Storage.Database;
using BookManagementSystem.Storage.Database.Entities;
using BookManagementSystem.ViewModels;
using Dasync.Collections;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;

namespace BookManagementSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IReadDatabaseRepository<BookEntity,string> _repository;
        private readonly IReadDatabaseRepository<CategoryEntity, int> _categoriesRepository;
        private readonly IReadDatabaseRepository<AuthorEntity, int> _authorsRepository;

        private readonly IEventsRepository _eventsRepository;
        private readonly ILogger<BooksController> _logger;
        private readonly IMediator _mediator;
        public BooksController(ILogger<BooksController> logger, IMediator mediator, IReadDatabaseRepository<BookEntity, string> repository, IEventsRepository eventsRepository, IReadDatabaseRepository<CategoryEntity, int> categoriesRepository, IReadDatabaseRepository<AuthorEntity, int> authorsRepository)
        {
            _logger = logger;
            _mediator = mediator;
            _repository = repository;
            _eventsRepository = eventsRepository;
            _categoriesRepository = categoriesRepository;
            _authorsRepository = authorsRepository;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<BookViewModel> Get(string id)
        {
            var bookEntity = await _repository.GetById(id);
            var bookViewModel = new BookViewModel()
            {
                Id = bookEntity.Id,
                Title = bookEntity.Title,
                Description = bookEntity.Description,
                Category = bookEntity.Category?.Name,
                Authors = bookEntity.Authors.Select(author=>author.Name)
            };
            return bookViewModel;
        }

        [HttpPost]
        [Route("")]
        public async Task<string> Create(CreateBookViewModel bookViewModel)
        {
            var id = await _repository.Count()+1;

            await _mediator.Send(new CreateBookCommand(id.ToString(), bookViewModel.Title,
                bookViewModel.Description, bookViewModel.Category, bookViewModel.Authors));
            
            return id.ToString();
        }


        [HttpGet]
        [Route("")]
        public async Task<IEnumerable<BookViewModel>> GetAll(int? page, int? pageSize)
        {
            IQueryable<BookEntity> books = _repository
                .All()
                .OrderByDescending(item => item.CreatedTime);
            if (page != null)
            {
                var skipped = (page.Value - 1) * pageSize.GetValueOrDefault(10);
                books = books.Skip(skipped).Take(pageSize.GetValueOrDefault(10));
            }

            var bookViewModel = await books
                .Select(bookEntity => new BookViewModel()
                {
                    Id = bookEntity.Id,
                    Title = bookEntity.Title,
                    Description = bookEntity.Description,
                    Category = bookEntity.Category.Name,
                    Authors = bookEntity.Authors.Select(author => author.Name)
                }).ToListAsync();
            return bookViewModel;
        }

        [HttpGet]
        [Route("{id}/events")]
        public async IAsyncEnumerable<EventViewModel> GetEvents(string id, long? page, int? pageSize,[FromQuery] EventsFilter filter =null)
        {
            bool FilterEvents(EventMetaData eventMetaData)
            {
                var isvalid = true;
                
                if (filter == null)
                    return true;
                
                if (!string.IsNullOrEmpty(filter.Type))
                    isvalid =  eventMetaData.Type.Contains(filter.Type);

                if (filter.FromDate.HasValue)
                    isvalid = isvalid && eventMetaData.CreateTime.Date >= filter.FromDate.GetValueOrDefault().Date;

                if (filter.ToDate.HasValue)
                    isvalid = isvalid && eventMetaData.CreateTime.Date <= filter.ToDate.GetValueOrDefault().Date;

                return isvalid;

            }

            var list = await _eventsRepository.GetEvents<BookAggregate>(id,page, pageSize, FilterEvents).ToListAsync();
            foreach (var item in list)
            {
                var evt = (BookEvents)await item;
                yield return new EventViewModel()
                {
                    Args = evt,
                    Event = evt.GetType().Name,
                    CreatedDate=evt.CreatedDate,
                    EventDescription = await GeEventDescription(evt)
                };
            }
        }

       

        [HttpPut]
        [Route("{id}/Title")]
        public async Task ChangeTitle(string id, ChangeTitle request)
        {
            var result = await _mediator.Send(new ChangeTitleCommand(Guid.NewGuid(), id.ToString(), request.Title));
        }

        [HttpPut]
        [Route("{id}/Description")]
        public async Task ChangeDescription(string id, ChangeDescription request)
        {
            var result = await _mediator.Send(new ChangeDescriptionCommand(Guid.NewGuid(), id, request.Description));
        }
        [HttpPut]
        [Route("{id}/Category")]
        public async Task ChangeCategory(string id, ChangeCategory request)
        {
            var result = await _mediator.Send(new ChangeCategoryCommand(Guid.NewGuid(), id, request.CategoryId));
        }

        [HttpPost]
        [HttpDelete]
        [Route("{id}/Authors")]
        public async Task ModifyAuthors(string id, ModifyAuthor request)
        {
            if (Request.Method == HttpMethods.Post)
                await _mediator.Send(new AddAuthorCommand(Guid.NewGuid(), id, request.AuthorId));
            else if (Request.Method == HttpMethods.Delete)
                await _mediator.Send(new RemoveAuthorCommand(Guid.NewGuid(), id, request.AuthorId));
        }

        public async Task<string> GeEventDescription(object data)
        {
            return data switch
            {
                BookEvents.ChangeTitle changeTitle => $"Title Changed to {changeTitle.Title}",
                BookEvents.ChangeDescription changeDescription => $"Description Changed to {changeDescription.Description}",
                BookEvents.ChangeCategory changeCategory => $"Category Changed to {(await _categoriesRepository.GetById(changeCategory.CategoryId))?.Name}",
                BookEvents.AddAuthor addAuthor => $"Author { (await _authorsRepository.GetById(addAuthor.AuthorId)).Name} added",
                BookEvents.RemoveAuthor removeAuthor => $"Author {(await _authorsRepository.GetById(removeAuthor.AuthorId)).Name} removed",
                _ => string.Empty
            };
        }


    }
}
