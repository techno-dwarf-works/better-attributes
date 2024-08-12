using System;
using System.Diagnostics;
using Better.Commons.Runtime.Drawers.Attributes;
using Better.Internal.Core.Runtime;

namespace Better.Attributes.Runtime.Validation
{
    public enum ValidationType
    {
        Info = 0,
        Warning = 1,
        Error = 2
    }
    
    [Conditional(Defines.Editor)]
    [AttributeUsage(AttributeTargets.Field)]
    public abstract class ValidationAttribute : MultiPropertyAttribute
    {
        public ValidationType ValidationType { get; set; } = ValidationType.Error;
    }
}