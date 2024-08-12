using System;
using System.Diagnostics;
using Better.Internal.Core.Runtime;

namespace Better.Attributes.Runtime.Select
{
    [Conditional(Defines.Editor)]
    [AttributeUsage(AttributeTargets.Field)]
    public class DropdownAttribute : BaseSelectAttribute
    {
        private readonly string _selector;

        public bool ShowDefault { get; set; } = true;
        public bool ShowUniqueKey { get; set; }

        public DropdownAttribute(string selector) : base(null)
        {
            _selector = selector;
        }

        public DropdownAttribute(string selector, DisplayName displayName) : base(null, displayName)
        {
            _selector = selector;
        }

        public DropdownAttribute(string selector, DisplayGrouping displayGrouping) : base(null, displayGrouping)
        {
            _selector = selector;
        }

        public string Selector => _selector;
    }
}