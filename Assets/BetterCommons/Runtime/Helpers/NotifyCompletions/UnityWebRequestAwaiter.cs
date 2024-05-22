using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine.Networking;

namespace Better.Commons.Runtime.Helpers.NotifyCompletions
{
    [DebuggerNonUserCode]
    public readonly struct UnityWebRequestAwaiter : INotifyCompletion
    {
        private readonly UnityWebRequestAsyncOperation _asyncOperation;

        public bool IsCompleted => _asyncOperation.isDone;

        public UnityWebRequestAwaiter(UnityWebRequestAsyncOperation asyncOperation)
        {
            _asyncOperation = asyncOperation;
        }

        public void OnCompleted(Action continuation)
        {
            _asyncOperation.completed += _ => continuation();
        }

        public UnityWebRequest GetResult()
        {
            return _asyncOperation.webRequest;
        }
    }
}