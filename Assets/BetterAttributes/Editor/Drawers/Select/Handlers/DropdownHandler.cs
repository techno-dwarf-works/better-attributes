using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Better.Attributes.EditorAddons.Comparers;
using Better.Attributes.EditorAddons.Extensions;
using Better.Attributes.Runtime;
using Better.Attributes.Runtime.Select;
using Better.Attributes.Runtime.Utilities;
using Better.Commons.EditorAddons.Drawers;
using Better.Commons.EditorAddons.Extensions;
using Better.Commons.EditorAddons.Utility;
using Better.Commons.Runtime.Comparers;
using Better.Commons.Runtime.Utility;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Better.Attributes.EditorAddons.Drawers.Select
{
    [HandlerBinding(typeof(DropdownAttribute))]
    public class DropdownHandler : BaseSelectHandler
    {
        private IDataCollection _collection = new NoneCollection();

        private HashSet<Type> _ignoreNameSplit = new HashSet<Type>()
        {
            typeof(float), typeof(decimal), typeof(double)
        };

        protected override void OnSetup()
        {
            var dropdownAttribute = (DropdownAttribute)Attribute;
            var selector = dropdownAttribute.Selector;
            var instance = PropertyContainer;
            
            if (!SelectorUtility.TryGetValue(selector, instance, out var value))
            {
                Container.GetOrAddHelpBox($"Can't get value from selector: {selector}", typeof(DropdownHandler), HelpBoxMessageType.Error);
            }

            switch (value)
            {
                case null:
                    _collection = new NoneCollection();
                    break;
                case IDictionary dictionary:
                    _collection = new DictionaryCollection(dictionary, dropdownAttribute.ShowDefault, dropdownAttribute.ShowUniqueKey);
                    break;
                case IList list:
                    _collection = new EnumerableCollection(list, dropdownAttribute.ShowDefault, dropdownAttribute.ShowUniqueKey);
                    break;
                case IEnumerable enumerable:
                    _collection = new EnumerableCollection(enumerable, dropdownAttribute.ShowDefault, dropdownAttribute.ShowUniqueKey);
                    break;
            }
        }

        public override object GetCurrentValue()
        {
            var property = Container.SerializedProperty;
            return property.GetValue();
        }

        protected override List<object> GetObjects()
        {
            return _collection.GetValues();
        }
        
        protected override GUIContent GetResolvedName(object value, DisplayName displayName)
        {
            if (value == null)
            {
                return new GUIContent(LabelDefines.Null);
            }

            var name = _collection.FindName(value);
            if (value is Object)
            {
                displayName = DisplayName.Full;
            }

            switch (displayName)
            {
                case DisplayName.Short:
                    if (_ignoreNameSplit.Contains(value.GetType()))
                    {
                        return new GUIContent(name);
                    }

                    var split = name.Split(SelectorUtility.NameSeparator);
                    return split.Length <= 1 ? new GUIContent(name) : new GUIContent(split.Last());
                case DisplayName.Full:
                    return new GUIContent(name);
                default:
                    DebugUtility.LogException<ArgumentOutOfRangeException>(nameof(displayName));
                    return null;
            }
        }

        protected override IEnumerable<GUIContent> GetResolvedGroupedName(object value, DisplayGrouping grouping)
        {
            if (value == null)
            {
                return new GUIContent[] { new GUIContent(LabelDefines.Null) };
            }

            var type = _collection.FindName(value);

            if (_ignoreNameSplit.Contains(value.GetType()))
            {
                return new GUIContent[] { new GUIContent(type) };
            }

            var split = type.Split(SelectorUtility.NameSeparator);
            if (split.Length <= 1)
            {
                return new GUIContent[] { new GUIContent(type) };
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


        public override bool IsSkippingFieldDraw()
        {
            return true;
        }

        public override string GetButtonText()
        {
            return _collection.FindName(CurrentValue);
        }

        protected override bool ResolveState(object iteratedValue)
        {
            return Equals(CurrentValue, iteratedValue);
        }

        public override bool ValidateSelected(object item)
        {
            var type = GetFieldOrElementType();

            if (item == null)
            {
                return true;
            }

            return TypeComparer.Instance.Equals(type, item.GetType());
        }

        public override void Update(object value)
        {
            var property = Container.SerializedProperty;
            if (!property.Verify()) return;
            property.SetValue(value);
            base.Update(value);
        }

        public override bool CheckSupported()
        {
            return true;
        }

        public override GUIContent GenerateHeader()
        {
            return new GUIContent("Select");
        }
    }
}