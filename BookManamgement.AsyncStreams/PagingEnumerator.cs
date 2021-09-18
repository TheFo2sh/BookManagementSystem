using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookManamgement.AsyncStreams
{
   internal class PagingEnumerator<T> : IAsyncEnumerator<T>
    {
        private Func<long, int, Task<T>> _func;
        private long cursor;
        private bool _isLastOne;
        private readonly Func<T, bool> _isLastOneFunc;

        public PagingEnumerator(Func<long, int, Task<T>> func, Func<T, bool> isLastOneFunc, long startPage = 0)
        {
            _func = func;
            _isLastOneFunc = isLastOneFunc;
            cursor = startPage;
        }

        public ValueTask DisposeAsync()
        {
            _func = null;
            return ValueTask.CompletedTask;
        }

        public async ValueTask<bool> MoveNextAsync()
        {
            if (_isLastOne)
                return false;

            Current = await _func(cursor, 10);

            cursor += 10;
            _isLastOne = _isLastOneFunc(Current);

            return true;
        }

        public T Current { get; private set; }
    }
}
