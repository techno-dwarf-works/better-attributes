using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Better.Commons.Runtime.Helpers.NotifyCompletions
{
    [DebuggerNonUserCode]
    public readonly struct AsyncOperationAwaiter : INotifyCompletion
    {
        private readonly AsyncOperation _asyncOperation;
        public bool IsCompleted => _asyncOperation.isDone;

        public AsyncOperationAwaiter(AsyncOperation asyncOperation)
        {
            _asyncOperation = asyncOperation;
        }

        public void OnCompleted(Action continuation)
        {
            _asyncOperation.completed += _ => continuation();
        }

        public AsyncOperation GetResult()
        {
            return _asyncOperation;
        }
    }
}