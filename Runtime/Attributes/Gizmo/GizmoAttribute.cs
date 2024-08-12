using System;
using System.Diagnostics;
using Better.Internal.Core.Runtime;
namespace Better.Attributes.Runtime.Gizmo
{
    /// <summary>
    /// Attribute to draw handles in scene view
    /// </summary>
    [Conditional(Defines.Editor)]
    [AttributeUsage(AttributeTargets.Field)]
    public class GizmoAttribute : BaseGizmoAttribute
    {
    }
}