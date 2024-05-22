using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Better.Attributes.EditorAddons.Drawers.Utility;
using Better.Attributes.EditorAddons.Extensions;
using Better.Attributes.Runtime.Select;
using Better.Commons.Runtime.Extensions;
using Better.Commons.Runtime.Utility;
using UnityEditor;
using UnityEngine;

namespace Better.Attributes.EditorAddons.Drawers.Select.SetupStrategies
{
    public class SelectSerializedTypeStrategy : SetupStrategy
    {
        public SelectSerializedTypeStrategy(FieldInfo fieldInfo, object propertyContainer, SelectAttributeBase selectAttributeBase) : base(fieldInfo, propertyContainer,
            selectAttributeBase)
        {
        }

        public override bool ResolveState(object currentValue, object iteratedValue)
        {
            if (iteratedValue == null && currentValue == null) return true;
            return iteratedValue is Type type && currentValue is Type currentType && currentType == type;
        }

        protected static bool ValidateInternal(Type type)
        {
            if (type.IsValueType)
            {
                return true;
            }

            var constructor = type.GetConstructor(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null, Type.EmptyTypes, null);

            return constructor != null;
        }

        public override bool IsSkipingFieldDraw()
        {
            return true;
        }

        public override bool Validate(object item)
        {
            if (item == null)
            {
                return true;
            }

            if (item is Type type)
            {
                return ValidateInternal(type);
            }

            return false;
        }

        public override bool CheckSupported()
        {
            return true;
        }

        public override GUIContent GenerateHeader()
        {
            return new GUIContent("Available Types");
        }

        public override GUIContent[] ResolveGroupedName(object value, DisplayGrouping grouping)
        {
            if (value == null)
            {
                return new GUIContent[] { new GUIContent(SelectUtility.Null) };
            }

            if (value is Type type)
            {
                if (string.IsNullOrEmpty(type.FullName))
                {
                    return new GUIContent[] { new GUIContent(type.Name) };
                }

                var split = type.FullName.Split(AttributesDefinitions.NameSeparator);
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

            return new GUIContent[] { new GUIContent(SelectUtility.NotSupported) };
        }

        public override GUIContent ResolveName(object value, DisplayName displayName)
        {
            if (value == null)
            {
                return new GUIContent(SelectUtility.Null);
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

            return new GUIContent(SelectUtility.NotSupported);
        }

        public override string GetButtonName(object currentValue)
        {
            if (currentValue is Type type)
            {
                return type.Name;
            }

            return SelectUtility.Null;
        }

        public override List<object> Setup()
        {
            var selectionObjects = GetFieldOrElementType().GetAllInheritedTypes().Cast<object>().ToList();
            selectionObjects.Insert(0, null);
            return selectionObjects;
        }
    }
}