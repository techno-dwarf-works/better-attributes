using System;
using System.Diagnostics;
using Better.Internal.Core.Runtime;

namespace Better.Attributes.Runtime.Manipulation
{
    /// <summary>
    /// Attribute to disable field editing in Inspector 
    /// </summary>
    [Conditional(Defines.Editor)]
    [AttributeUsage(AttributeTargets.Field)]
    public class ReadOnlyAttribute : ManipulateAttribute
    {
        public ReadOnlyAttribute() : base(ManipulationMode.Disable)
        {
        }
    }
}