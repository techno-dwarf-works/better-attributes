using System.Reflection;
using Better.Internal.Core.Runtime;
using UnityEditor;
using UnityEngine;

namespace Better.Commons.EditorAddons.Utility
{
    public static class PropertyFieldUtility
    {
        private static MethodInfo _defaultPropertyField;

        static PropertyFieldUtility()
        {
            var type = typeof(EditorGUI);
            _defaultPropertyField = type.GetMethod("DefaultPropertyField", Defines.MethodFlags);
        }
        
        public static bool PropertyFieldSafe(Rect position, SerializedProperty property, GUIContent label)
        {
            return (bool)_defaultPropertyField.Invoke(null, new object[] { position, property, label });
        }
    }
}