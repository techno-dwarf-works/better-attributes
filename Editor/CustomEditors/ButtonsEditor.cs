using Better.Attributes.EditorAddons.Drawers.EditorButton;
using Better.Commons.EditorAddons.CustomEditors.Attributes;
using Better.Commons.EditorAddons.CustomEditors.Base;
using UnityEditor;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Better.Attributes.EditorAddons.CustomEditors
{
    [MultiEditor(typeof(Object), true, Order = 999)]
    public class ButtonsEditor : ExtendedEditor
    {
        public ButtonsEditor(Object target, SerializedObject serializedObject) : base(target, serializedObject)
        {
        }

        public override void OnDisable()
        {
        }

        public override void OnEnable()
        {
        }

        public override VisualElement CreateInspectorGUI()
        {
            var container = new EditorButtonDrawer(_serializedObject);
            container.CreateFromTarget(_target);
            return container;
        }

        public override void OnChanged(SerializedObject serializedObject)
        {
        }
    }
}