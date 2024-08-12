using System;
using System.Diagnostics;
using Better.Internal.Core.Runtime;

namespace Better.Attributes.Runtime.Manipulation
{
    [Conditional(Defines.Editor)]
    [AttributeUsage(AttributeTargets.Field)]
    public abstract class ManipulateUserConditionAttribute : ManipulateAttribute
    {
        public string MemberName { get; }
        public object MemberValue { get; }
        
        public ManipulateUserConditionAttribute(string memberName, object memberValue, ManipulationMode modeType) : base(modeType)
        {
            MemberName = memberName;
            MemberValue = memberValue;
            order = -999;
        }
    }
}