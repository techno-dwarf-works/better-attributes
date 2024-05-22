using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Better.Commons.Runtime.Helpers.NotifyCompletions
{
    [DebuggerNonUserCode]
    public readonly struct ResourceRequestAwaiter : INotifyCompletion
    {
        private readonly ResourceRequest _asyncOperation;
        public bool IsCompleted => _asyncOperation.isDone;

        public ResourceRequestAwaiter(ResourceRequest asyncOperation)
        {
            _asyncOperation = asyncOperation;
        }

        public void OnCompleted(Action continuation)
        {
            _asyncOperation.completed += _ => continuation();
        }

        public ResourceRequest GetResult()
        {
            return _asyncOperation;
        }
    }
}