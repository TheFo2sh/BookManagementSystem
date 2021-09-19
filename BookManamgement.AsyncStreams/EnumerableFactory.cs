using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManamgement.AsyncStreams
{
    public static class EnumerableFactory
    {
        public static IAsyncEnumerable<T> Create<T>(Func<long, int, Task<T>> generator, Func<long, T, bool> isEnd, long startPage, int pageSize)
        {
            return new PagingEnumerable<T>(generator, isEnd, startPage, pageSize);
        }
       
    }
}
