using System;
using System.Diagnostics;
using Better.Internal.Core.Runtime;

namespace Better.Attributes.Runtime.Validation
{
    [Conditional(Defines.Editor)]
    [AttributeUsage(AttributeTargets.Field)]
    public class Clamp01Attribute : ClampAttribute
    {
        public Clamp01Attribute() : base(0f, 1f)
        {
        }
    }
}