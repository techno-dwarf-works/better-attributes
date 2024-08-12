using System;
using System.Diagnostics;
using Better.Commons.Runtime.Drawers.Attributes;
using Better.Internal.Core.Runtime;

namespace Better.Attributes.Runtime.Preview
{
    /// <summary>
    /// Attribute for preview in Inspector.
    /// Click on question mark or label to see preview.
    /// </summary>
    [Conditional(Defines.Editor)]
    [AttributeUsage(AttributeTargets.Field)]
    public class PreviewAttribute : MultiPropertyAttribute
    {
        private readonly float _previewSize;

        public float PreviewSize => _previewSize;

        public PreviewAttribute()
        {
            _previewSize = 150f;
        }

        public PreviewAttribute(float previewSize)
        {
            _previewSize = previewSize;
        }
    }
}