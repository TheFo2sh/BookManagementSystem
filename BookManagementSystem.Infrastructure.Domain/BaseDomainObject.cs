﻿using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Dasync.Collections;

namespace BookManagementSystem.Infrastructure.Domain
{
    public abstract class BaseDomainObject<TEventsHandler, TState> where TEventsHandler : new()
    {
        private readonly IEventsRepository _eventsRepository;
        private readonly TEventsHandler _handler;
       
        protected ConcurrentStack<TState> StateStore;
        
        public string AggregateId { get; }
        public long Position { get; private set; }

        protected BaseDomainObject(string aggregateId, TState state, IEventsRepository eventsRepository)
        {
            _eventsRepository = eventsRepository;
            _handler = new TEventsHandler();
            StateStore = new ConcurrentStack<TState>(new[] { state });
            AggregateId = aggregateId;

        }

        protected TState GetCurrentState()
        {
            return StateStore.TryPeek(out TState oldState) ? oldState : default;
        }

        protected async Task Apply(object evt)
        {
            var method = typeof(TEventsHandler).GetMethod("Handle", new[] { evt.GetType(), typeof(TState), typeof(CancellationToken) });
            
            if (method == null)
                throw new NotSupportedException($"{evt.GetType()} is has no registered handler in {typeof(TEventsHandler)}");
            
            var newState = await (Task<TState>)method.Invoke(_handler, new[] { evt, GetCurrentState(), CancellationToken.None });
            
            StateStore.Push(newState);
        }

        protected async Task Apply<T>(T evt)
        {
            if (_handler is IEventsHandler<T, TState> eventsHandler)
                StateStore.Push(await eventsHandler.Handle(evt, GetCurrentState(), CancellationToken.None));
            else
                throw new NotSupportedException($"{evt.GetType()} is has no registered handler in {typeof(TEventsHandler)}");
        }
        
        public async Task<long> CommitAsync(object evt)
        {
            Position = await _eventsRepository.CommitAsync(this.GetType().Name, AggregateId, evt);
            var currentState = GetCurrentState();
            StateStore = new ConcurrentStack<TState>(new[] { currentState });
            OnStateCommited(currentState, Position);

            return Position;
        }

        public async Task Rehydrate()
        {
            var events = _eventsRepository.GetEvents(this.GetType().Name, AggregateId);
            if (events == null)
                return;
            await events.ForEachAsync(async evt => await Apply(await evt));

        }
        protected async Task<long> ApplyAndCommitAsync<T>(T evt)
        {
            await Apply(evt);
            try
            {
                Position = await CommitAsync(evt);
            }
            catch (Exception)
            {
                Undo();
                throw;
            }
            return Position;
        }

        protected void Undo()
        {
            if (StateStore.Count > 1)
                StateStore.TryPop(out _);
        }

        protected virtual void OnStateCommited(TState state, long position)
        {
        }

    }
}
