using System;
using System.Diagnostics;
using Better.Internal.Core.Runtime;
using UnityEngine;

namespace Better.Attributes.Runtime.Headers
{
    /// <summary>
    /// Draws texture by path in Unity Inspector
    /// </summary>
    /// <remarks>Quick way to use this attribute -> ContextMenu on Texture Importer -> Convert To IconHeaderAttribute</remarks>
    [Conditional(Defines.Editor)]
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class IconHeaderAttribute : PropertyAttribute
    {
        public string Guid { get; }
        public bool UseTransparency { get; set; } = true;

        public IconHeaderAttribute(string assetGuid)
        {
            Guid = assetGuid;
        }
    }
}