using System;
using System.Collections.Generic;
using System.Linq;
using Better.Attributes.EditorAddons.Extensions;
using Better.Attributes.Runtime;
using Better.Attributes.Runtime.Select;
using Better.Attributes.Runtime.Utilities;
using Better.Commons.EditorAddons.Utility;
using Better.Commons.Runtime.Utility;
using UnityEngine;

namespace Better.Attributes.EditorAddons.Drawers.Select
{
    public abstract class BaseSelectTypeHandler : BaseSelectHandler
    {
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

        protected abstract IEnumerable<Type> GetInheritedTypes(Type fieldType);

        protected sealed override List<object> GetObjects()
        {
            var fieldOrElementType = GetFieldOrElementType();
            var types = GetInheritedTypes(fieldOrElementType);
            var selectionObjects = types.Cast<object>().ToList();
            selectionObjects.Insert(0, null);
            return selectionObjects;
        }
    }
}