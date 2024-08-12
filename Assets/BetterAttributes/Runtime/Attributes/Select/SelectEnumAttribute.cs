using System;
using System.Diagnostics;
using Better.Internal.Core.Runtime;

namespace Better.Attributes.Runtime.Select
{
    /// <summary>
    /// Attribute for Implementation selection in Inspector.
    /// Use in pair with [SerializeReference] Attribute.
    /// </summary>
    [Conditional(Defines.Editor)]
    [AttributeUsage(AttributeTargets.Field)]
    [Obsolete("Use SelectAttribute instead", true)]
    public class SelectEnumAttribute : BaseSelectAttribute
    {
        public SelectEnumAttribute(Type type) : base(type)
        {
        }

        public SelectEnumAttribute(Type type, DisplayName displayName) : base(type, displayName)
        {
        }

        public SelectEnumAttribute(DisplayName displayName) : base(displayName)
        {
        }

        public SelectEnumAttribute()
        {
        }
    }
}