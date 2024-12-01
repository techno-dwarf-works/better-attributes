using Better.Attributes.Runtime;
using Better.Attributes.Runtime.Misc;
using Better.Attributes.Runtime.Utilities;
using Better.Commons.EditorAddons.Drawers;
using Better.Commons.EditorAddons.Helpers;

namespace Better.Attributes.EditorAddons.Drawers.Misc
{
    [HandlerBinding(typeof(CustomTooltipAttribute))]
    public class CustomToolTipHandler : MiscLabelHandler
    {
        protected override void OnUpdateLabel(LabelContainer labelContainer)
        {
            var tooltipAttribute = (CustomTooltipAttribute)_attribute;
            var symbol = tooltipAttribute.TooltipSymbol;
            if (tooltipAttribute.TooltipSymbol == char.MinValue)
            {
                symbol = LabelDefines.Asterisk;
            }

            labelContainer.Suffix += symbol;
            labelContainer.Tooltip = tooltipAttribute.Tooltip;
        }
    }
}