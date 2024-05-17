using System.Reflection;
using Better.Attributes.Runtime.Misc;
using Better.Commons.EditorAddons.Drawers.Caching;
using Better.Commons.EditorAddons.Drawers.Utility;
using Better.Commons.EditorAddons.Utility;
using UnityEditor;
using UnityEngine;

namespace Better.Attributes.EditorAddons.Drawers.Misc.Wrappers
{
    public abstract class MiscWrapper : SerializedPropertyHandler
    {
        protected SerializedProperty _property;
        protected MiscAttribute _attribute;
        protected FieldInfo _fieldInfo;

        public override void Deconstruct()
        {
        }

        public virtual void SetProperty(SerializedProperty property, FieldInfo fieldInfo, MiscAttribute attribute)
        {
            _property = property;
            _attribute = attribute;
            _fieldInfo = fieldInfo;
        }

        public abstract void PreDraw(Rect position, GUIContent label);

        public virtual void DrawField(Rect rect, GUIContent label)
        {
            EditorGUI.PropertyField(rect, _property, label, true);
        }

        public abstract void PostDraw();
        public abstract HeightCacheValue GetHeight(GUIContent label);
    }
}