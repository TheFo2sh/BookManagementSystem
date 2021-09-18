using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using EventStore.ClientAPI;

namespace BookManagementSystem.Factories
{
    public static class EventStoreConnectionFactory
    {
        public static IEventStoreConnection Create()
        {
            var consetting = ConnectionSettings.Create().DisableTls().Build();

            var eventStoreConnection =
                EventStoreConnection.Create(consetting, new IPEndPoint(IPAddress.Loopback, 1113));
            eventStoreConnection.ConnectAsync();
            return eventStoreConnection;
        }
    }
}
