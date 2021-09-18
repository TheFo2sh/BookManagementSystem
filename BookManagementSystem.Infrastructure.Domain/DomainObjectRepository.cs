using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dasync.Collections;

namespace BookManagementSystem.Infrastructure.Domain
{
    public class DomainObjectRepository<T, TEventsHandler, TState> where T:BaseDomainObject<TEventsHandler, TState> where TEventsHandler : new()
    {
        private readonly IEventsRepository _eventsRepository;

        public DomainObjectRepository(IEventsRepository eventsRepository)
        {
            _eventsRepository = eventsRepository;
        }

        public async Task<T> GetAsync(string aggregateId)
        {
            var aggregate = (T) Activator.CreateInstance(typeof(T), aggregateId, _eventsRepository);
            var events = _eventsRepository.GetEvents(typeof(T).Name, aggregateId);
            await events.ForEachAsync(evt => aggregate.Apply(evt));
            return aggregate;
        }
    }
}
