using System;
using System.Diagnostics;
using Better.Internal.Core.Runtime;
using UnityEngine;

namespace Better.Commons.Runtime.Drawers.Attributes
{
    [Conditional(Defines.Editor)]
    [AttributeUsage(AttributeTargets.Field)]
    public abstract class MultiPropertyAttribute : PropertyAttribute
    {
        
    }
}