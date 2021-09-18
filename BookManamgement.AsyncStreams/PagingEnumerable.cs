using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BookManamgement.AsyncStreams
{
    internal class PagingEnumerable<T> : IAsyncEnumerable<T>
    {
        private Func<long, int, Task<T>> _func;
        private readonly long _startPage;
        private readonly Func<T, bool> _isLastOneFunc;

        public PagingEnumerable(Func<long, int, Task<T>> func, Func<T, bool> isLastOneFunc, long startPage)
        {
            _func = func;
            _isLastOneFunc = isLastOneFunc;
            _startPage = startPage;
        }

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = new CancellationToken())
        {
            return new PagingEnumerator<T>(_func, _isLastOneFunc, _startPage);
        }
    }
}