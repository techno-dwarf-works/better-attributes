using System;
using System.Diagnostics;
using Better.Internal.Core.Runtime;

namespace Better.Attributes.Runtime.Manipulation
{
    [Conditional(Defines.Editor)]
    [AttributeUsage(AttributeTargets.Field)]
    public class ShowInEditorModeAttribute : ManipulateAttribute
    {
        public ShowInEditorModeAttribute() : base(ManipulationMode.Show)
        {
        }
    }
}