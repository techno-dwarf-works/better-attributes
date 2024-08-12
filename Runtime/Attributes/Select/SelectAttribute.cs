using System;
using System.Diagnostics;
using Better.Internal.Core.Runtime;

namespace Better.Attributes.Runtime.Select
{
    /// <summary>
    /// Attribute for selection in Inspector.
    /// Use in pair with [SerializeReference] Attribute.
    /// </summary>
    [Conditional(Defines.Editor)]
    [AttributeUsage(AttributeTargets.Field)]
    public class SelectAttribute : BaseSelectAttribute
    {
        public SelectAttribute(Type type) : base(type)
        {
        }

        public SelectAttribute(Type type, DisplayName displayName) : base(type, displayName)
        {
        }

        public SelectAttribute(Type type, DisplayGrouping displayGrouping) : base(type, displayGrouping)
        {
        }

        public SelectAttribute(DisplayName displayName) : base(displayName)
        {
        }

        public SelectAttribute(DisplayGrouping displayGrouping) : base(displayGrouping)
        {
        }

        public SelectAttribute()
        {
        }
    }
}