using System;
using System.Diagnostics;
using Better.Internal.Core.Runtime;

namespace Better.Attributes.Runtime.Manipulation
{
    [Conditional(Defines.Editor)]
    [AttributeUsage(AttributeTargets.Field)]
    public class ShowIfAttribute : ManipulateUserConditionAttribute
    {
        public ShowIfAttribute(string memberName, object memberValue) : base(memberName, memberValue, ManipulationMode.Show)
        {
        }
    }
}