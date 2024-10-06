using System;
using System.Diagnostics;
using Better.Internal.Core.Runtime;

namespace Better.Attributes.Runtime
{
    /// <summary>
    /// Displays Button in Inspector
    /// </summary>
    [Conditional(Defines.Editor)]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class EditorButtonAttribute : Attribute
    {
        private readonly string _displayName;

        public EditorButtonAttribute() : this(string.Empty)
        {
        }
        
        public EditorButtonAttribute(string displayName)
        {
            _displayName = displayName;
            Priority = -1;
            CaptureGroup = -1;
        }

        public int Priority { get; set; }

        public int CaptureGroup { get; set; }

        private bool IsValidName()
        {
            return !string.IsNullOrWhiteSpace(_displayName);
        }

        public string GetDisplayName(string name)
        {
            return IsValidName() ? _displayName : name;
        }
    }
}