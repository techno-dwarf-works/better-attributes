using System.Threading;
using UnityEngine.Events;

namespace Better.Commons.Runtime.Helpers.CompletionAwaiters
{
    public class UnityEventCompletionAwaiter<T> : CompletionAwaiter<UnityEvent<T>, T>
    {
        public UnityEventCompletionAwaiter(UnityEvent<T> source, CancellationToken cancellationToken)
            : base(source, cancellationToken)
        {
            Source.AddListener(OnSourceInvoked);
        }

        private void OnSourceInvoked(T value) => SetResult(value);
        protected override void OnCompleted(T result) => Source.RemoveListener(OnSourceInvoked);
    }

    public class UnityEventCompletionAwaiter : CompletionAwaiter<UnityEvent, bool>
    {
        public UnityEventCompletionAwaiter(UnityEvent source, CancellationToken cancellationToken)
            : base(source, cancellationToken)
        {
            Source.AddListener(OnSourceInvoked);
        }

        private void OnSourceInvoked() => SetResult(true);
        protected override void OnCompleted(bool result) => Source.RemoveListener(OnSourceInvoked);
    }
}