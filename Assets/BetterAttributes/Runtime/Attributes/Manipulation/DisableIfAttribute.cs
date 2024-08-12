using System;
using System.Diagnostics;
using Better.Internal.Core.Runtime;

namespace Better.Attributes.Runtime.Manipulation
{
    [Conditional(Defines.Editor)]
    [AttributeUsage(AttributeTargets.Field)]
    public class DisableIfAttribute : ManipulateUserConditionAttribute
    {
        public DisableIfAttribute(string memberName, object memberValue) : base(memberName, memberValue, ManipulationMode.Disable)
        {
        }
    }
}