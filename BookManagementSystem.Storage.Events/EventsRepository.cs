using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using BookManagementSystem.Infrastructure.Domain;
using BookManamgement.AsyncStreams;
using Dasync.Collections;
using EventStore.ClientAPI;

namespace BookManagementSystem.Storage.Events
{
   public class EventsRepository : IEventsRepository
    {
        private IEventStoreConnection _eventStoreConnection;

        public EventsRepository(IEventStoreConnection eventStoreConnection)
        {
            _eventStoreConnection = eventStoreConnection;
        }

      
        private async Task<object> Parse(RecordedEvent evt)
        {
            await using var memoryStream = new MemoryStream(evt.Data);
            var type = Type.GetType(evt.EventType);
            if (type == null)
                return null;
            return await JsonSerializer.DeserializeAsync(memoryStream, type);
        }

        public IAsyncEnumerable<Task<object>> GetEvents(string aggregatetype, string aggregateId)
        {
            Task<StreamEventsSlice> LoadEvents(long start, int count) => _eventStoreConnection.ReadStreamEventsForwardAsync($"{aggregatetype}_{aggregateId}", start, count, false);

            var slices = EnumerableFactory.Create(LoadEvents, slice => slice.IsEndOfStream, 0);

            var events = slices.SelectMany(slice => slice.Events).Select(x => x.Event).Select(Parse);

            return events;

        }

        public async Task<long> CommitAsync(string aggregatetype, string aggregateId, object evt)
        {
            var eventData = new EventData(Guid.NewGuid(), evt.GetType().FullName, true, JsonSerializer.SerializeToUtf8Bytes(evt), null);
            var result = await _eventStoreConnection.AppendToStreamAsync($"{aggregatetype}_{aggregateId}", ExpectedVersion.Any, eventData);
            return result.LogPosition.CommitPosition;
        }
    }

}
