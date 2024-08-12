using System;
using System.Diagnostics;
using Better.Internal.Core.Runtime;

namespace Better.Attributes.Runtime.Manipulation
{
    [Conditional(Defines.Editor)]
    [AttributeUsage(AttributeTargets.Field)]
    public class DisableInPlayModeAttribute : ManipulateAttribute
    {
        public DisableInPlayModeAttribute() : base(ManipulationMode.Disable)
        {
        }
    }
}