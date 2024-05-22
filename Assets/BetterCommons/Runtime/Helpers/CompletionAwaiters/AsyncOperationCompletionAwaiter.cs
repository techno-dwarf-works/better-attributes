using System;
using System.Threading;
using Better.Commons.Runtime.Extensions;
using UnityEngine;
using ThreadingTask = System.Threading.Tasks.Task;

namespace Better.Commons.Runtime.Helpers.CompletionAwaiters
{
    public class AsyncOperationCompletionAwaiter : CompletionAwaiter<AsyncOperation, bool>
    {
        private readonly IProgress<float> _progress;

        public AsyncOperationCompletionAwaiter(AsyncOperation source, IProgress<float> progress = null)
            : base(source, CancellationToken.None)
        {
            _progress = progress;
            ProcessAsync();
        }

        private async void ProcessAsync()
        {
            while (!Source.IsRelativeCompleted())
            {
                _progress?.Report(Source.progress);
                await ThreadingTask.Yield();
            }

            SetResult(true);
        }

        protected override void OnCompleted(bool result)
        {
            _progress?.Report(1f);
        }
    }
}