using System;
using System.Diagnostics;
using Better.Internal.Core.Runtime;

namespace Better.Attributes.Runtime.Validation
{
    [Conditional(Defines.Editor)]
    [AttributeUsage(AttributeTargets.Field)]
    public class MaxAttribute : ValidationAttribute
    {
        public float Max { get; }

        public MaxAttribute(float max)
        {
            Max = max;
        }
    }
}