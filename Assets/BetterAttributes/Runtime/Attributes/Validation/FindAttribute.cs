using System;
using System.Diagnostics;
using Better.Internal.Core.Runtime;

namespace Better.Attributes.Runtime.Validation
{
    public enum RequireDirection
    {
        Parent,
        None,
        Child
    }

    [Conditional(Defines.Editor)]
    [AttributeUsage(AttributeTargets.Field)]
    public class FindAttribute : ValidationAttribute
    {
        public Type RequiredType { get; }
        public RequireDirection RequireDirection { get; set; } = RequireDirection.None;
        public bool ValidateIfFieldEmpty { get; set; } = true;

        public FindAttribute(Type type)
        {
            RequiredType = type;
        }

        public FindAttribute() :this(null)
        {
        }
    }
}