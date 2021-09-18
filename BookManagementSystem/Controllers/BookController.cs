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
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BookManagementSystem.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {
        private readonly DomainObjectRepository<BookAggregate, BookEventHandler, BookState> _repository;

        private readonly ILogger<BookController> _logger;
        private readonly IMediator _mediator;
        public BookController(ILogger<BookController> logger, IMediator mediator, DomainObjectRepository<BookAggregate, BookEventHandler, BookState> repository)
        {
            _logger = logger;
            _mediator = mediator;
            _repository = repository;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<BookState> Get(string id)
        {
            var aggregate = await _repository.GetAsync(id);
            return aggregate.GetState();
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

      

    }
}
