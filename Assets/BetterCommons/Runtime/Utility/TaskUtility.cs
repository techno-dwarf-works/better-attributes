using System;
using System.Threading;
using System.Threading.Tasks;

namespace Better.Commons.Runtime.Utility
{
    public static class TaskUtility
    {
        public static Task WaitForSeconds(float seconds, CancellationToken cancellationToken = default)
        {
            var millisecondsDelay = TimeUtility.SecondsToMilliseconds(seconds);
            return Task.Delay(millisecondsDelay, cancellationToken);
        }

        public static async Task WaitWhile(Func<bool> condition, CancellationToken cancellationToken = default)
        {
            if (condition == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(condition));
                return;
            }

            while (!cancellationToken.IsCancellationRequested && condition.Invoke())
            {
                await Task.Yield();
            }
        }

        public static async Task WaitUntil(Func<bool> condition, CancellationToken cancellationToken = default)
        {
            if (condition == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(condition));
                return;
            }

            while (!cancellationToken.IsCancellationRequested && !condition.Invoke())
            {
                await Task.Yield();
            }
        }

        public static async Task WaitFrame(int count, CancellationToken cancellationToken = default)
        {
            if (count < 1)
            {
                var message = $"{nameof(count)} cannot be less 1";
                DebugUtility.LogException<ArgumentOutOfRangeException>(message);
                return;
            }

            for (int i = 0; i < count; i++)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                await Task.Yield();
            }
        }
    }
}