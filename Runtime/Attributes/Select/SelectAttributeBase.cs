using System;
using System.Diagnostics;
using Better.Commons.Runtime.Drawers.Attributes;
using Better.Internal.Core.Runtime;

namespace Better.Attributes.Runtime.Select
{
    public enum DisplayName
    {
        Short,
        Full,
    }

    public enum DisplayGrouping
    {
        None = 1,
        Grouped = 2 ,
        GroupedFlat = 3
    }

    [Conditional(Defines.Editor)]
    [AttributeUsage(AttributeTargets.Field)]
    public abstract class BaseSelectAttribute : MultiPropertyAttribute
    {
        private readonly Type _type;
        public DisplayName DisplayName { get; } = DisplayName.Short;
        public DisplayGrouping DisplayGrouping { get; } = DisplayGrouping.None;

        protected BaseSelectAttribute(Type type)
        {
            _type = type;
        }

        protected BaseSelectAttribute(Type type, DisplayName displayName)
        {
            _type = type;
            DisplayName = displayName;
        }

        protected BaseSelectAttribute(Type type, DisplayGrouping displayGrouping)
        {
            _type = type;
            DisplayName = DisplayName.Short;
            DisplayGrouping = displayGrouping;
        }

        protected BaseSelectAttribute(DisplayName displayName)
        {
            DisplayName = displayName;
        }

        protected BaseSelectAttribute(DisplayGrouping displayGrouping)
        {
            DisplayName = DisplayName.Short;
            DisplayGrouping = displayGrouping;
        }

        public Type GetFieldType()
        {
            return _type;
        }

        protected BaseSelectAttribute()
        {
        }
    }
}