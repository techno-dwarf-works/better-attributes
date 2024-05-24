using System;
using System.Collections.Generic;
using System.Reflection;
using Better.Attributes.Runtime.Select;
using Better.Commons.EditorAddons.Utility;
using Better.Commons.Runtime.Extensions;
using UnityEditor;
using UnityEngine;

namespace Better.Attributes.EditorAddons.Drawers.Select.SetupStrategies
{
    public abstract class SetupStrategy
    {
        private protected readonly FieldInfo _fieldInfo;
        private protected readonly object _propertyContainer;
        private protected readonly SelectAttributeBase _selectAttributeBase;

        protected SetupStrategy(FieldInfo fieldInfo, object propertyContainer, SelectAttributeBase selectAttributeBase)
        {
            _fieldInfo = fieldInfo;
            _propertyContainer = propertyContainer;
            _selectAttributeBase = selectAttributeBase;
        }

        public abstract List<object> Setup();
        public abstract GUIContent ResolveName(object value, DisplayName displayName);
        public abstract GUIContent[] ResolveGroupedName(object value, DisplayGrouping grouping);
        public abstract string GetButtonName(object currentValue);
        public abstract bool ResolveState(object currentValue, object iteratedValue);
        public abstract bool Validate(object item);
        public abstract bool CheckSupported();
        public abstract GUIContent GenerateHeader();
        public abstract bool IsSkipingFieldDraw();

        public virtual Type GetFieldOrElementType()
        {
            var t = _selectAttributeBase.GetFieldType();
            if (t != null)
            {
                return t;
            }

            var fieldType = _fieldInfo.FieldType;
            if (fieldType.IsArrayOrList())
                return fieldType.GetCollectionElementType();
            return fieldType;
        }

        public virtual void DrawField(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
    }
}