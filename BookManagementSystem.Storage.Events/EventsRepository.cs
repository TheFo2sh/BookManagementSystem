using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public IAsyncEnumerable<Task<object>> GetEvents<T>(string aggregateId,long? pageNumber, int? pageSize, string eventName = null)
        {
            return GetEvents(typeof(T).Name, aggregateId, pageNumber,pageSize,eventName);
        }

        public IAsyncEnumerable<Task<object>> GetEvents(string aggregatetype, string aggregateId)
        {
            return GetEvents(aggregatetype, aggregateId, null,null);
        }

        public IAsyncEnumerable<Task<object>> GetEvents(string aggregatetype, string aggregateId, long? page,
            int? pageSize, string eventName = null)
        {
            Task<StreamEventsSlice> LoadEvents(long start, int count) =>
                _eventStoreConnection.ReadStreamEventsForwardAsync($"{aggregatetype}_{aggregateId}", start, count,
                    false);

            bool IsEnd(long index, StreamEventsSlice slice) =>
                slice.IsEndOfStream || (page.HasValue && index >= page * pageSize);

            IAsyncEnumerable<StreamEventsSlice> slices = EnumerableFactory
                .Create(LoadEvents, IsEnd,
                    page.GetValueOrDefault(0), pageSize.GetValueOrDefault(10));
           
            var events = slices.SelectMany(slice => slice.Events).Select(x => x.Event);
            if (!string.IsNullOrEmpty(eventName))
            {
                events = events.Where(ev =>
                {
                    return ev.EventType.Contains(eventName);
                });
            }

            return events.Select(Parse);

        }

        public async Task<long> CommitAsync(string aggregatetype, string aggregateId, object evt)
        {
            var eventType = evt.GetType();
            var eventData = new EventData(Guid.NewGuid(), $"{eventType.FullName}, {eventType.Assembly.FullName}", true, JsonSerializer.SerializeToUtf8Bytes(evt), null);
           
            var result = await _eventStoreConnection.AppendToStreamAsync($"{aggregatetype}_{aggregateId}", ExpectedVersion.Any, eventData);
            
            return result.LogPosition.CommitPosition;
        }
    }

}
