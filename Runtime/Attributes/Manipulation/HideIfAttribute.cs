using System;
using System.Diagnostics;
using Better.Internal.Core.Runtime;

namespace Better.Attributes.Runtime.Manipulation
{
    [Conditional(Defines.Editor)]
    [AttributeUsage(AttributeTargets.Field)]
    public class HideIfAttribute : ManipulateUserConditionAttribute
    {
        public HideIfAttribute(string memberName, object memberValue) : base(memberName, memberValue, ManipulationMode.Hide)
        {
        }
    }
}