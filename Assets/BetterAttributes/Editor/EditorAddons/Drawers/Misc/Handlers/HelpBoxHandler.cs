using Better.Attributes.Runtime.Misc;
using Better.Commons.EditorAddons.Utility;
using UnityEngine.UIElements;

namespace Better.Attributes.EditorAddons.Drawers.Misc
{
    public class HelpBoxHandler : MiscHandler
    {
        protected override void OnSetupContainer()
        {
            var helpBoxAttribute = (HelpBoxAttribute)_attribute;

            _container.GetOrAddHelpBox(helpBoxAttribute.Text, nameof(HelpBoxHandler), HelpBoxMessageType.Info);
        }
    }
}