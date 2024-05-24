using Better.Attributes.Runtime.Misc;
using Better.Commons.EditorAddons.Utility;
using UnityEditor.UIElements;
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

            propertyElement.schedule.Execute(() => UpdateLabel(propertyElement, tooltipAttribute)).Until(() => propertyElement.Q<Label>() != null);
        }

        private void UpdateLabel(PropertyField propertyElement, CustomTooltipAttribute tooltipAttribute)
        {
            var label = propertyElement.Q<Label>();
            if (label == null) return;
            label.text += Asterisk;
            label.tooltip = tooltipAttribute.Tooltip;
        }
    }
}