using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Better.Commons.Runtime.Helpers.NotifyCompletions
{
    [DebuggerNonUserCode]
    public readonly struct AssetBundleRequestAwaiter : INotifyCompletion
    {
        private readonly AssetBundleRequest _asyncOperation;
        public bool IsCompleted => _asyncOperation.isDone;

        public AssetBundleRequestAwaiter(AssetBundleRequest asyncOperation)
        {
            _asyncOperation = asyncOperation;
        }

        public void OnCompleted(Action continuation)
        {
            _asyncOperation.completed += _ => continuation();
        }

        public Object GetResult()
        {
            return _asyncOperation.asset;
        }
    }
}