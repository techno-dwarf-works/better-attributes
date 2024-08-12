using System;
using System.Diagnostics;
using Better.Internal.Core.Runtime;
using UnityEngine;

namespace Better.Attributes.Runtime.Headers
{
    /// <summary>
    /// Replacement for Header("State")
    /// </summary>
    [Conditional(Defines.Editor)]
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class StateHeaderAttribute : HeaderAttribute
    {
        public StateHeaderAttribute() : base("State")
        {
        }
    }
}