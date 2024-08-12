using System;
using System.Diagnostics;
using Better.Commons.Runtime.Drawers.Attributes;
using Better.Internal.Core.Runtime;

namespace Better.Attributes.Runtime.Manipulation
{
    public enum ManipulationMode
    {
        Show,
        Hide,
        Disable,
        Enable
    }
    
    [Conditional(Defines.Editor)]
    [AttributeUsage(AttributeTargets.Field)]
    public abstract class ManipulateAttribute : MultiPropertyAttribute
    {
        public ManipulationMode ModeType { get; }

        public ManipulateAttribute(ManipulationMode modeType)
        {
            ModeType = modeType;
            order = -999;
        }
    }
}