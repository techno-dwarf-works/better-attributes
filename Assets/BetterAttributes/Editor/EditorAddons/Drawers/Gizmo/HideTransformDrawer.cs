using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Better.Attributes.EditorAddons.Drawers.Gizmo
{
    public class HideTransformButtonUtility
    {
        public VisualElement DrawHideTransformButton()
        {
            var button = new Button();
            UpdateButtonText(button);
            UpdateButtonText(button);
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