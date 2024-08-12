using System;
using System.Diagnostics;
using Better.Internal.Core.Runtime;

namespace Better.Attributes.Runtime.Misc
{
    [Conditional(Defines.Editor)]
    [AttributeUsage(AttributeTargets.Field)]
    public class CustomTooltipAttribute : MiscAttribute
    {
        public string Tooltip { get; }
        
        public char TooltipSymbol { get; set; }

        public CustomTooltipAttribute(string tooltip)
        {
            Tooltip = tooltip;
            order = -999;
        }
    }
}