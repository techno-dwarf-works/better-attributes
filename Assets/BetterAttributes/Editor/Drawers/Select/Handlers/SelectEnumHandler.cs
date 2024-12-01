using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Better.Attributes.EditorAddons.Comparers;
using Better.Attributes.EditorAddons.Extensions;
using Better.Attributes.Runtime;
using Better.Attributes.Runtime.Select;
using Better.Attributes.Runtime.Utilities;
using Better.Commons.EditorAddons.Drawers;
using Better.Commons.EditorAddons.Extensions;
using Better.Commons.Runtime.Extensions;
using Better.Commons.Runtime.Utility;
using UnityEngine;
using UnityEngine.UIElements;

namespace Better.Attributes.EditorAddons.Drawers.Select
{
    [HandlerBinding(typeof(Enum), typeof(SelectAttribute))]
    public class SelectEnumHandler : BaseSelectHandler
    {
        private Type _enumType;
        private bool _isFlag;
        private PredefinedValues _none = new PredefinedValues(LabelDefines.NoneEnumName, EnumUtility.DefaultIntFlag);
        private PredefinedValues _everythingValue = new PredefinedValues(LabelDefines.EverythingEnumName, -1);

        private struct PredefinedValues
        {
            public PredefinedValues(string name, int value)
            {
                Name = name;
                Value = value;
            }

            public string Name { get; }
            public int Value { get; }
        }

        protected override void OnSetup()
        {
            var enumType = FieldInfo.FieldType;
            if (enumType.IsArrayOrList())
            {
                enumType = enumType.GetCollectionElementType();
            }

            var everythingValue = EnumUtility.EverythingFlag(enumType).ToFlagInt();
            _everythingValue = new PredefinedValues(LabelDefines.EverythingEnumName, everythingValue);

            _enumType = GetFieldOrElementType();
            _isFlag = _enumType.GetCustomAttribute<FlagsAttribute>() != null;
        }

        public override string GetButtonText()
        {
            var intValue = (int)CurrentValue;
            if (_isFlag)
            {
                if (intValue.Equals(_none.Value))
                {
                    return _none.Name;
                }

                if (intValue.Equals(_everythingValue.Value))
                {
                    return _everythingValue.Name;
                }
            }

            var value = Enum.ToObject(_enumType, intValue);
            return value.ToString();
        }

        protected override bool ResolveState(object iteratedValue)
        {
            if (CurrentValue is int intCurrent && iteratedValue is int intIterated)
            {
                if (!_isFlag) return intCurrent == intIterated;

                if (intIterated == _none.Value || intIterated == _everythingValue.Value)
                {
                    return intCurrent == intIterated;
                }

                return (intCurrent & intIterated) != 0;
            }

            return false;
        }

        public override bool ValidateSelected(object item)
        {
            return true;
        }

        public override bool CheckSupported()
        {
            return GetFieldOrElementType().IsEnum;
        }

        public override GUIContent GenerateHeader()
        {
            return new GUIContent("Options");
        }

        public override bool IsSkippingFieldDraw()
        {
            return true;
        }

        protected override IEnumerable<GUIContent> GetResolvedGroupedName(object value, DisplayGrouping grouping)
        {
            return new GUIContent[] { new GUIContent(LabelDefines.NotSupported) };
        }

        protected override List<object> GetObjects()
        {
            _isFlag = _enumType.GetCustomAttribute<FlagsAttribute>() != null;

            var ints = EnumUtility.GetAllValues(_enumType).Select(x => x.ToFlagInt()).ToList();
            if (_isFlag)
            {
                _none = new PredefinedValues(LabelDefines.NoneEnumName, EnumUtility.DefaultIntFlag);
                if (!ints.Contains(_none.Value))
                {
                    ints.Insert(0, _none.Value);
                }

                var everything = EnumUtility.EverythingFlag(_enumType).ToFlagInt();
                _everythingValue = new PredefinedValues(LabelDefines.EverythingEnumName, everything);
                if (!ints.Contains(_everythingValue.Value))
                {
                    ints.Insert(ints.Count, _everythingValue.Value);
                }
            }

            return ints.Cast<object>().ToList();
        }

        protected override GUIContent GetResolvedName(object value, DisplayName displayName)
        {
            if (value is int intValue)
            {
                var eEnum = Enum.ToObject(_enumType, intValue);

                if (intValue == _everythingValue.Value)
                {
                    return new GUIContent(_everythingValue.Name);
                }

                if (intValue == _none.Value)
                {
                    return new GUIContent(_none.Name);
                }

                switch (displayName)
                {
                    case DisplayName.Short:
                        return new GUIContent($"{eEnum}");
                    case DisplayName.Full:
                        return new GUIContent($"{_enumType.Name}.{eEnum}");
                    default:
                        DebugUtility.LogException<ArgumentOutOfRangeException>(nameof(displayName));
                        return null;
                }
            }

            return new GUIContent(LabelDefines.NotSupported);
        }

        public override void Update(object value)
        {
            var property = Container.SerializedProperty;
            if (!property.Verify()) return;
            var intValue = (int)value;
            var currentValue = property.intValue;
            currentValue = EnumCalculator.CalculateCurrentValue(currentValue, _isFlag, intValue, _everythingValue.Value);

            property.intValue = currentValue;
            base.Update(value);
        }

        public override object GetCurrentValue()
        {
            var property = Container.SerializedProperty;
            return property.intValue;
        }
    }
}