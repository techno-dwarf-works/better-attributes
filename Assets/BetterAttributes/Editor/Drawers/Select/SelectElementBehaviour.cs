using Better.Commons.EditorAddons.Drawers.BehavioredElements;
using Better.Commons.EditorAddons.Enums;
using Better.Commons.EditorAddons.Extensions;
using Better.Commons.Runtime.Extensions;
using UnityEngine.UIElements;

namespace Better.Attributes.EditorAddons.Drawers.Select
{
    public class SelectElementBehaviour : DefaultElementBehaviour<Button>
    {
        public override void OnLink(BehavioredElement<Button> behavioredElement)
        {
            base.OnLink(behavioredElement);

            var icon = BehavioredElement.SubElement.AddIcon(IconType.GrayDropdown);
            icon.style.AlignSelf(new StyleEnum<Align>(Align.FlexEnd));
        }
    }
}