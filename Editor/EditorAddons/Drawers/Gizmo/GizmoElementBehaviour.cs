using Better.Commons.EditorAddons.Drawers.BehavioredElements;
using Better.Commons.Runtime.Extensions;
using UnityEngine.UIElements;

namespace Better.Attributes.EditorAddons.Drawers.Gizmo
{
    public class GizmoElementBehaviour : DefaultElementBehaviour<Button>
    {
        public override void OnLink(BehavioredElement<Button> behavioredElement)
        {
            base.OnLink(behavioredElement);
            BehavioredElement.style
                .FlexGrow(new StyleFloat(1f));
            BehavioredElement.SubElement.style.FlexGrow(new StyleFloat(1f));
        }
        
        public override void OnAttach(VisualElement root)
        {
            if (root.Contains(BehavioredElement)) return;
            BehavioredElement.RemoveFromHierarchy();

            root.Add(BehavioredElement);
        }
    }
}