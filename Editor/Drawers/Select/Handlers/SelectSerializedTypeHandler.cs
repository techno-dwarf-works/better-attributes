using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Better.Attributes.EditorAddons.Comparers;
using Better.Attributes.EditorAddons.Extensions;
using Better.Attributes.Runtime;
using Better.Attributes.Runtime.DrawInspector;
using Better.Attributes.Runtime.Select;
using Better.Attributes.Runtime.Utilities;
using Better.Commons.EditorAddons.Drawers;
using Better.Commons.EditorAddons.Extensions;
using Better.Commons.EditorAddons.Utility;
using Better.Commons.Runtime.DataStructures.SerializedTypes;
using Better.Commons.Runtime.Extensions;
using Better.Commons.Runtime.Utility;
using UnityEditor;
using UnityEngine;

namespace Better.Attributes.EditorAddons.Drawers.Select
{
    
    [HandlerBinding(typeof(SerializedType), typeof(SelectAttribute))]
    public class SelectSerializedTypeHandler : BaseSelectTypeHandler
    {
        protected override void OnSetup()
        {
            var property = Container.SerializedProperty;
            if (property.IsArrayElement())
            {
                var index = property.GetArrayIndex();
                Container.LabelContainer.Text = $"Element {index}";
            }
        }

        public override void Update(object value)
        {
            var property = Container.SerializedProperty;
            if (!property.Verify()) return;
            var typeValue = (Type)value;
            if (property.propertyType == SerializedPropertyType.Generic)
            {
                var buffer = typeValue != null ? new SerializedType(typeValue) : new SerializedType();
                property.SetValue(buffer);
            }
            else if (property.propertyType == SerializedPropertyType.ManagedReference)
            {
                var buffer = typeValue == null ? null : Activator.CreateInstance(typeof(SerializedType), typeValue);
                property.managedReferenceValue = buffer;
            }

            base.Update(value);
        }

        public override object GetCurrentValue()
        {
            var property = Container.SerializedProperty;
            var objectOfProperty = property.GetValue();
            if (objectOfProperty == null)
            {
                return new SerializedType();
            }

            var type = objectOfProperty.GetType();
            if (type == typeof(SerializedType))
            {
                type = (objectOfProperty as SerializedType)?.Type;
            }

            return type;
        }

        protected override bool ResolveState(object iteratedValue)
        {
            if (iteratedValue == null && CurrentValue == null) return true;
            return iteratedValue is Type type && CurrentValue is Type currentType && currentType == type;
        }

        public override bool IsSkippingFieldDraw()
        {
            return true;
        }

        public override bool ValidateSelected(object item)
        {
            return true;
        }

        public override bool CheckSupported()
        {
            return true;
        }

        public override GUIContent GenerateHeader()
        {
            return new GUIContent("Available Types");
        }

        protected override IEnumerable<GUIContent> GetResolvedGroupedName(object value, DisplayGrouping grouping)
        {
            if (value == null)
            {
                return new GUIContent[] { new GUIContent(LabelDefines.Null) };
            }

            if (value is Type type)
            {
                if (string.IsNullOrEmpty(type.FullName))
                {
                    return new GUIContent[] { new GUIContent(type.Name) };
                }

                var split = type.FullName.Split(SelectorUtility.NameSeparator);
                if (split.Length <= 1)
                {
                    return new GUIContent[] { new GUIContent(type.Name) };
                }

                switch (grouping)
                {
                    case DisplayGrouping.Grouped:
                        return split.Select(x => new GUIContent(x)).ToArray();
                    case DisplayGrouping.GroupedFlat:
                        return new GUIContent[] { new GUIContent(split.First()), new GUIContent(split.Last()) };
                    default:
                        DebugUtility.LogException<ArgumentOutOfRangeException>(nameof(grouping));
                        return Array.Empty<GUIContent>();
                }
            }

            return new GUIContent[] { new GUIContent(LabelDefines.NotSupported) };
        }

        protected override GUIContent GetResolvedName(object value, DisplayName displayName)
        {
            if (value == null)
            {
                return new GUIContent(LabelDefines.Null);
            }

            if (value is Type type)
            {
                switch (displayName)
                {
                    case DisplayName.Short:
                        return new GUIContent(type.Name);
                    case DisplayName.Full:
                        return new GUIContent(type.FullName);
                    default:
                        DebugUtility.LogException<ArgumentOutOfRangeException>(nameof(displayName));
                        return null;
                }
            }

            return new GUIContent(LabelDefines.NotSupported);
        }

        public override string GetButtonText()
        {
            if (CurrentValue is Type type)
            {
                return type.Name;
            }

            return LabelDefines.Null;
        }

        protected override IEnumerable<Type> GetInheritedTypes(Type fieldType)
        {
            return fieldType.GetAllInheritedTypes();
        }
    }
}