using UnityEngine;

namespace Better.Commons.Runtime.Helpers.YieldInstructions
{
    public abstract class YieldInstruction<T> : CustomYieldInstruction
    {
        protected T Source { get; }

        public YieldInstruction(T source)
        {
            Source = source;
        }
    }
}