using System;
using System.Reflection;
using Better.Commons.EditorAddons.Extensions;
using Better.Commons.EditorAddons.Utility;
using Better.Commons.Runtime.Extensions;
using Better.Commons.Runtime.Utility;
using UnityEditor.UIElements;
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

            var propertyField = _container.CoreElement.Q<PropertyField>();
            var indexOf = _container.CoreElement.IndexOf(propertyField);
            propertyField.style.SetVisible(false);
            var labelElement = new Label(propertyField.label);
            _container.CoreElement.Insert(indexOf, labelElement);

            _isFlag = enumType.GetCustomAttribute<FlagsAttribute>() != null;
            _everythingValue = EnumUtility.EverythingFlag(enumType).ToFlagInt();

            var currentValue = EnumUtility.ToEnum(enumType, _container.SerializedProperty.intValue);

            var elements = Enum.GetValues(_fieldInfo.FieldType);
            var toolbar = new Toolbar();


            var toolbarElement = _container.CreateElementFrom(toolbar);
            toolbarElement.style
                .FlexDirection(new StyleEnum<FlexDirection>(FlexDirection.Row))
                .FlexWrap(new StyleEnum<Wrap>(Wrap.Wrap))
                .FlexGrow(StyleDefinition.OneStyleFloat);
            
            foreach (Enum element in elements)
            {
                var toolbarToggle = new ToolbarToggle();
                toolbarToggle.label = element.ToString();
                toolbarToggle.value = EnumUtility.HasValue(currentValue, element, _isFlag);
                toolbarToggle.style
                    .Height(StyleDefinition.SingleLineHeight)
                    .FlexGrow(StyleDefinition.OneStyleFloat)
                    .AlignSelf(new StyleEnum<Align>(Align.Center))
                    .BorderWidth(new StyleFloat(0f));
                
                toolbarToggle.RegisterCallback<ChangeEvent<bool>, Enum>(OnValueChanged, element);
                toolbarToggle.AddToClassList(Button.ussClassName);
                
                toolbarToggle.labelElement.style
                    .FlexGrow(StyleDefinition.OneStyleFloat);
                
                toolbarToggle.Query<VisualElement>()
                    .Class(ToolbarToggle.inputUssClassName)
                    .First().style
                    .SetVisible(false);
                toolbarElement.Add(toolbarToggle);
            }

            toolbarElement.AddTag(typeof(Toolbar));
        }

        private void OnValueChanged(ChangeEvent<bool> clickEvent, Enum enumValue)
        {
            var property = _container.SerializedProperty;
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