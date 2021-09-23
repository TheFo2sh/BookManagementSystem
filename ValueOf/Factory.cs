using System;
using System.Threading.Tasks;

namespace ValueOf
{
    public static class Factory
    {
        private static IServiceProvider _serviceProvider;

        public static void SetServiceProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }


        public static async Task<TOwner> CreateFromAsync<TValue, TOwner>(TValue value) where TOwner : ValueOf<TValue, TOwner>
        {
            
            var obj = (TOwner)Microsoft.Extensions.DependencyInjection.ActivatorUtilities.CreateInstance(_serviceProvider, typeof(TOwner), value);
            if (await obj.Validate(value))
                return obj;

            throw await obj.OnInValid(value);
        }

        public static async Task<TOwner> CreateFromAsync<TOwner>(int value) where TOwner : ValueOf<int, TOwner>
        {
            return await CreateFromAsync<int, TOwner>(value);
        }

    }
}