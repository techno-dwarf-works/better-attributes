using System;
using Better.Attributes.Runtime.Misc;
using Better.Commons.EditorAddons.Extensions;
using Better.Commons.EditorAddons.Utility;
using UnityEngine.UIElements;

namespace Better.Attributes.EditorAddons.Drawers.Misc
{
    public class HelpBoxHandler : MiscHandler
    {
        protected override void OnSetupContainer()
        {
            var helpBoxAttribute = (HelpBoxAttribute)_attribute;

            var textOrSelector = helpBoxAttribute.Text;

            var instance = _container.SerializedProperty.GetLastNonCollectionContainer();
            
            if (SelectorUtility.TryGetValue(textOrSelector, instance, out var value))
            {
                textOrSelector = value.ToString();
            }

            _container.GetOrAddHelpBox(textOrSelector, nameof(HelpBoxHandler), HelpBoxMessageType.Info);
        }
    }
}