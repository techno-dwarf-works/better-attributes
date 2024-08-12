using System;
using System.Diagnostics;
using Better.Internal.Core.Runtime;

namespace Better.Attributes.Runtime.Manipulation
{
    [Conditional(Defines.Editor)]
    [AttributeUsage(AttributeTargets.Field)]
    public class HideInPlayModeAttribute : ManipulateAttribute
    {
        public HideInPlayModeAttribute() : base(ManipulationMode.Hide)
        {
        }
    }
}