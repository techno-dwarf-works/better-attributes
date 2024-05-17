using System.Reflection;
using Better.Attributes.Runtime.Misc;
using Better.Commons.EditorAddons.Drawers.Caching;
using Better.Commons.EditorAddons.Utility;
using UnityEditor;
using UnityEngine;

namespace Better.Attributes.EditorAddons.Drawers.Misc.Wrappers
{
    public class HelpBoxWrapper : MiscWrapper
    {
        private HelpBoxAttribute _helpBoxAttribute;
        private GUIContent _helpBoxContent;
        private float _propertyHeight;
        private float _helpBoxHeight;

        public override void SetProperty(SerializedProperty property, FieldInfo fieldInfo, MiscAttribute attribute)
        {
            base.SetProperty(property, fieldInfo, attribute);
            _helpBoxAttribute = (HelpBoxAttribute)attribute;
            _helpBoxContent = new GUIContent(_helpBoxAttribute.Text);
        }

        public override void PreDraw(Rect position, GUIContent label)
        {
            var copy = new Rect(position);
            copy.height = _helpBoxHeight;
            copy.y += _propertyHeight + EditorGUIUtility.standardVerticalSpacing;

            VisualElementUtility.HelpBox(copy, _helpBoxContent);
        }

        public override void PostDraw()
        {
        }

        public override HeightCacheValue GetHeight(GUIContent label)
        {
            _propertyHeight = EditorGUI.GetPropertyHeight(_property, label);
            _helpBoxHeight = VisualElementUtility.GetHelpBoxHeight(_helpBoxContent);
            return HeightCacheValue.GetFull(_propertyHeight + _helpBoxHeight + EditorGUIUtility.standardVerticalSpacing);
        }
    }
}