using Better.Commons.EditorAddons.Utility;
using Better.Commons.Runtime.Extensions;
using UnityEditor;
using UnityEngine.UIElements;

namespace Better.Attributes.EditorAddons.Drawers.Gizmo
{
    public class HideTransformButtonHelper
    {
        public VisualElement DrawHideTransformButton()
        {
            var button = new Button();
            button.name = nameof(HideTransformButtonHelper);
            UpdateButtonText(button);
            UpdateButtonText(button);
            button.style.FlexGrow(StyleDefinition.OneStyleFloat);
            button.RegisterCallback<ClickEvent, Button>(OnClicked, button);
            return button;
        }

        private static void UpdateButtonText(Button button)
        {
            var text = Tools.hidden ? GizmoDrawer.Show : GizmoDrawer.Hide;
            button.text = text;
        }

        private void OnClicked(ClickEvent clickEvent, Button button)
        {
            Tools.hidden = !Tools.hidden;
            UpdateButtonText(button);
            SceneView.RepaintAll();
        }
    }
}