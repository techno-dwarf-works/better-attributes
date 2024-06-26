﻿using System;
using Better.Commons.EditorAddons.Extensions;
using Better.Commons.Runtime.DataStructures.SerializedTypes;
using UnityEditor;

namespace Better.Attributes.EditorAddons.Drawers.Select.Wrappers
{
    public class SelectSerializedTypeWrapper : BaseSelectWrapper
    {

        public override void Update(object value)
        {
            if (!_property.Verify()) return;
            var typeValue = (Type)value;
            if (_property.propertyType == SerializedPropertyType.Generic)
            {
                var buffer = typeValue != null ? new SerializedType(typeValue) : new SerializedType();
                _property.SetValue(buffer);
            }
            else if (_property.propertyType == SerializedPropertyType.ManagedReference)
            {
                _property.managedReferenceValue = typeValue == null ? null : Activator.CreateInstance(typeof(SerializedType), typeValue);
            }
        }

        protected override float GetPropertyHeight(SerializedProperty copy)
        {
            return EditorGUIUtility.singleLineHeight;
        }

        public override object GetCurrentValue()
        {
            var objectOfProperty = _property.GetValue();
            var type = objectOfProperty.GetType();
            if (type == typeof(SerializedType))
            {
                type = (objectOfProperty as SerializedType)?.Type;
            }

            return type;
        }
    }
}