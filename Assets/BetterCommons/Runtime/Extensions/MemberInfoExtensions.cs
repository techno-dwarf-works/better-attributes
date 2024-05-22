using System;
using System.Reflection;
using Better.Commons.Runtime.Utility;

namespace Better.Commons.Runtime.Extensions
{
    public static class MemberInfoExtensions
    {
        public static string PrettyMemberName(this MemberInfo self)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return string.Empty;
            }
            
            return self.Name.PrettyCamelCase();
        }
    }
}