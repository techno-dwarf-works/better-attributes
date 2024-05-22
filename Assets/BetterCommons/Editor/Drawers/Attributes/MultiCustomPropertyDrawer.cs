using System;
using System.Diagnostics;
using Better.Internal.Core.Runtime;

namespace Better.Commons.EditorAddons.Drawers.Attributes
{
    [Conditional(Defines.Editor)]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class MultiCustomPropertyDrawer : Attribute
    {
        public Type ForAttribute { get; }
        public bool Override { get; set; }

        public MultiCustomPropertyDrawer(Type forAttribute)
        {
            ForAttribute = forAttribute;
        }
    }
}