using Better.Attributes.Runtime.Misc;
using Better.Commons.EditorAddons.Utility;
using UnityEngine.UIElements;

namespace Better.Attributes.EditorAddons.Drawers.Misc
{
    public class CustomToolTipHandler : MiscHandler
    {
        private const char Asterisk = '*';

        protected override void OnSetupContainer()
        {
            if (!_container.TryGetPropertyField(out var propertyElement))
            {
                return;
            }
            
            var tooltipAttribute = (CustomTooltipAttribute)_attribute;
            
            var label = propertyElement.Q<Label>();
            if (label == null) return;
            label.tooltip = tooltipAttribute.Tooltip;
            label.name += Asterisk;
        }
    }
}