using System.Collections.Generic;
using System.Threading.Tasks;

namespace Better.Commons.Runtime.Extensions
{
    public static class TaskExtensions
    {
        public static async void Forget(this Task self)
        {
            await self;
        }
        
        public static async void Forget<T>(this Task<T> self)
        {
            await self;
        }

        public static Task WhenAll(this IEnumerable<Task> self)
        {
            return Task.WhenAll(self);
        }

        public static Task<T[]> WhenAll<T>(this IEnumerable<Task<T>> self)
        {
            return Task.WhenAll(self);
        }

        public static Task WhenAny(this IEnumerable<Task> self)
        {
            return Task.WhenAny(self);
        }

        public static async Task<T> WhenAny<T>(this IEnumerable<Task<T>> self)
        {
            var task = await Task.WhenAny(self);
            return await task;
        }
    }
}