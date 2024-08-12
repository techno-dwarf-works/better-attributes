using System;
using System.Diagnostics;
using Better.Internal.Core.Runtime;

namespace Better.Attributes.Runtime.Manipulation
{
    [Conditional(Defines.Editor)]
    [AttributeUsage(AttributeTargets.Field)]
    public class EnableIfAttribute : ManipulateUserConditionAttribute
    {
        public EnableIfAttribute(string memberName, object memberValue) : base(memberName, memberValue, ManipulationMode.Enable)
        {
        }
    }
}