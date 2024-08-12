using System;
using System.Diagnostics;
using Better.Internal.Core.Runtime;

namespace Better.Attributes.Runtime.Misc
{
    [Conditional(Defines.Editor)]
    [AttributeUsage(AttributeTargets.Field)]
    public class RenameFieldAttribute : MiscAttribute
    {
        public string Name { get; }
        
        public RenameFieldAttribute(string name)
        {
            Name = name;
        }
    }
}