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
        private readonly Func<long,T, bool> _isLastOneFunc;
        private readonly int _pageSize;

        public PagingEnumerable(Func<long, int, Task<T>> func, Func<long,T, bool> isLastOneFunc, long startPage, int pageSize)
        {
            _func = func;
            _isLastOneFunc = isLastOneFunc;
            _startPage = startPage;
            _pageSize = pageSize;
        }

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = new CancellationToken())
        {
            return new PagingEnumerator<T>(_func, _isLastOneFunc, _pageSize, _startPage);
        }
    }
}