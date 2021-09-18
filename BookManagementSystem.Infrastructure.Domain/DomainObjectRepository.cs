using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dasync.Collections;
using MediatR;

namespace BookManagementSystem.Infrastructure.Domain
{
    public class DomainObjectRepository<T, TEventsHandler, TState> where T:BaseDomainObject<TEventsHandler, TState> where TEventsHandler : new()
    {
        private readonly IEventsRepository _eventsRepository;
        private readonly IMediator  _mediator;
        public DomainObjectRepository(IEventsRepository eventsRepository, IMediator mediator)
        {
            _eventsRepository = eventsRepository;
            _mediator = mediator;
        }

        public async Task<T> GetAsync(string aggregateId)
        {
            var aggregate = (T) Activator.CreateInstance(typeof(T), aggregateId, _eventsRepository, _mediator);
            var events = _eventsRepository.GetEvents(typeof(T).Name, aggregateId);
            await events.ForEachAsync(async evt =>
            {
                var e = await evt;
                await aggregate.Apply(e);
            });
            return aggregate;
        }
    }
}
