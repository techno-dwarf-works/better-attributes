using System.Threading;
using System.Threading.Tasks;

namespace Better.Commons.Runtime.Helpers.CompletionAwaiters
{
    public abstract class CompletionAwaiter<TSource, TValue> : CompletionAwaiter<TValue>
    {
        protected TSource Source { get; }
        
        protected CompletionAwaiter(TSource source, CancellationToken cancellationToken) : base(cancellationToken)
        {
            Source = source;
        }
    }
    
    public abstract class CompletionAwaiter<TValue>
    {
        private readonly TaskCompletionSource<TValue> _completionSource;
        public Task<TValue> Task => _completionSource.Task;

        public CompletionAwaiter(CancellationToken cancellationToken)
        {
            _completionSource = new();
            cancellationToken.Register(Cancel);

            if (cancellationToken.IsCancellationRequested)
            {
                SetResult(default);
            }
        }

        protected void SetResult(TValue value)
        {
            if (!_completionSource.TrySetResult(value))
            {
                OnCompleted(value);
            }
        }

        private void Cancel()
        {
            SetResult(default);
        }

        protected abstract void OnCompleted(TValue result);
    }
}