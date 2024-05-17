using System.Reflection;
using Better.Attributes.EditorAddons.Drawers.Gizmo;
using Better.Attributes.Runtime.Gizmo;
using Better.Commons.EditorAddons.CustomEditors.Attributes;
using Better.Commons.EditorAddons.CustomEditors.Base;
using Better.Commons.EditorAddons.Extensions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Better.Attributes.EditorAddons.CustomEditors
{
    [MultiEditor(typeof(Object), true, Order = -999)]
    public class GizmosEditor : ExtendedEditor
    {
        private HideTransformButtonUtility _hideTransformDrawer;

        public GizmosEditor(Object target, SerializedObject serializedObject) : base(target, serializedObject)
        {
        }

        public override void OnDisable()
        {
        }

        public override void OnEnable()
        {
            CheckAttribute();
        }

        private void CheckAttribute()
        {
            var attributeFound = IsAttributeFound();

            if (attributeFound && !(_serializedObject.targetObject is ScriptableObject))
            {
                _hideTransformDrawer = new HideTransformButtonUtility();
            }
        }

        private bool IsAttributeFound()
        {
            var iterator = _serializedObject.GetIterator().Copy();
            var attributeFound = false;
            while (iterator.Next(true))
            {
                var data = iterator.GetFieldInfoAndStaticTypeFromProperty();
                if (data == null)
                {
                    continue;
                }

                if (data.FieldInfo.GetCustomAttribute<GizmoLocalAttribute>() == null && data.FieldInfo.GetCustomAttribute<GizmoAttribute>() == null) continue;
                attributeFound = true;
                break;
            }

            return attributeFound;
        }

        public override VisualElement CreateInspectorGUI()
        {
            if (_hideTransformDrawer != null)
            {
                return _hideTransformDrawer.DrawHideTransformButton();
            }

            return null;
        }

        public override void OnChanged(SerializedObject serializedObject)
        {
            CheckAttribute();
        }
    }
}