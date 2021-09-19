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
        private readonly Func<long,T, bool> _isLastOneFunc;
        private readonly int _pageSize;
        public PagingEnumerator(Func<long, int, Task<T>> func, Func<long,T, bool> isLastOneFunc, int pageSize=10, long startPage = 0)
        {
            _func = func;
            _isLastOneFunc = isLastOneFunc;
            _pageSize = pageSize;
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

            Current = await _func(cursor, _pageSize);

            cursor += _pageSize;
            _isLastOne = _isLastOneFunc(cursor, Current);

            return true;
        }

        public T Current { get; private set; }
    }
}
