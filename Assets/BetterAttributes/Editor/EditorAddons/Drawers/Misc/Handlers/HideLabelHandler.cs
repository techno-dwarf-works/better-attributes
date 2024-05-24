using Better.Commons.EditorAddons.Utility;
using Better.Commons.Runtime.Extensions;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Better.Attributes.EditorAddons.Drawers.Misc
{
    public class HideLabelHandler : MiscHandler
    {
        protected override void OnSetupContainer()
        {
            if (!_container.TryGetPropertyField(out var propertyElement))
            {
                return;
            }

            var property = _container.Property;

            if (!property.hasVisibleChildren)
            {
                propertyElement.schedule.Execute(() => UpdateLabel(propertyElement)).Until(() => propertyElement.Q<Label>() != null);
            }

            propertyElement.style.SetVisible(false);
        }

        private void UpdateLabel(PropertyField propertyElement)
        {
            var label = propertyElement.Q<Label>();
            if (label == null) return;
            label.style.SetVisible(false);
        }
    }
}