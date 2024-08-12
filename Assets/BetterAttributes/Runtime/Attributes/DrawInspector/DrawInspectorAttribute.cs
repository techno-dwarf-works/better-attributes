using System;
using System.Diagnostics;
using Better.Commons.Runtime.Drawers.Attributes;
using Better.Internal.Core.Runtime;

namespace Better.Attributes.Runtime.DrawInspector
{
    /// <summary>
    /// Replaces object field with nested inspector
    /// </summary>
    [Conditional(Defines.Editor)]
    [AttributeUsage(AttributeTargets.Field)]
    public class DrawInspectorAttribute : MultiPropertyAttribute
    {
    }
}