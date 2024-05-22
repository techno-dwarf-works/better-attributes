using Better.Commons.EditorAddons.Utility;
using Better.Commons.Runtime.Extensions;
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
                var label = propertyElement.Q<Label>();
                label?.style.SetVisible(false);
            }

            propertyElement.style.SetVisible(false);
        }
    }
}