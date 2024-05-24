using System;
using System.Collections.Generic;
using System.Reflection;
using Better.Attributes.EditorAddons.Extensions;
using Better.Attributes.Runtime.Misc;
using Better.Commons.EditorAddons.Drawers.Caching;
using Better.Commons.EditorAddons.Helpers;
using Better.Commons.EditorAddons.Utility;
using Better.Commons.Runtime.Extensions;
using Better.Commons.Runtime.Utility;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Better.Attributes.EditorAddons.Drawers.Misc
{
    public class EnumButtonsHandler : MiscHandler
    {
        private Enum _enum;
        private bool _isFlag;
        private int _everythingValue;

        protected override void OnSetupContainer()
        {
            var enumType = _fieldInfo.FieldType;

            if (enumType.IsArrayOrList())
            {
                enumType = enumType.GetCollectionElementType();
            }

            if (!_container.TryGetByTag(_container.Property, out var fieldElement))
            {
                return;
            }

            if (fieldElement.Elements.TryFind(out PropertyField propertyField))
            {
                var indexOf = fieldElement.Elements.IndexOf(propertyField);
                propertyField.style.SetVisible(false);
                var labelElement = new Label(propertyField.label);
                fieldElement.Elements.Insert(indexOf, labelElement);
            }

            _isFlag = enumType.GetCustomAttribute<FlagsAttribute>() != null;
            _everythingValue = EnumUtility.EverythingFlag(enumType).ToFlagInt();

            var currentValue = EnumUtility.ToEnum(enumType, _container.Property.intValue);

            var elements = Enum.GetValues(_fieldInfo.FieldType);
            var toolbar = new Toolbar();

            foreach (Enum element in elements)
            {
                var toolbarToggle = new ToolbarToggle();
                toolbarToggle.label = element.ToString();
                toolbarToggle.value = EnumUtility.HasValue(currentValue, element, _isFlag);
                toolbarToggle.RegisterCallback<ChangeEvent<bool>, Enum>(OnValueChanged, element);
            }

            var toolbarElement = _container.CreateElementFrom(toolbar);
            toolbarElement.AddTag(typeof(Toolbar));
        }

        private void OnValueChanged(ChangeEvent<bool> clickEvent, Enum enumValue)
        {
            var property = _container.Property;
            var currentValue = EnumUtility.ToEnum(_fieldInfo.FieldType, property.intValue);
            Enum value;
            if (clickEvent.newValue)
            {
                value = EnumUtility.Add(currentValue, enumValue);
            }
            else
            {
                value = EnumUtility.Remove(currentValue, enumValue);
            }

            property.intValue = value.ToFlagInt();
            property.serializedObject.ApplyModifiedProperties();
        }
    }
}