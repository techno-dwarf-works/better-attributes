using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Better.Attributes.EditorAddons.Drawers.Utility;
using Better.Attributes.EditorAddons.Extensions;
using Better.Attributes.Runtime.Select;
using Better.Commons.Runtime.Extensions;
using Better.Commons.Runtime.Utility;
using UnityEngine;

namespace Better.Attributes.EditorAddons.Drawers.Select.SetupStrategies
{
    public class SelectEnumStrategy : SetupStrategy
    {
        private Type _enumType;
        private bool _isFlag;
        private PredefinedValues _none = new PredefinedValues(Constants.NoneEnumName, EnumUtility.DefaultIntFlag);
        private PredefinedValues _everythingValue = new PredefinedValues(Constants.EverythingEnumName, -1);
        private List<object> _enumValues;

        public SelectEnumStrategy(FieldInfo fieldInfo, object propertyContainer, SelectAttributeBase selectAttributeBase) : base(fieldInfo, propertyContainer,
            selectAttributeBase)
        {
        }

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

        public override bool IsSkipingFieldDraw()
        {
            return true;
        }

        public override string GetButtonName(object currentValue)
        {
            var intValue = (int)currentValue;
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

        public override bool ResolveState(object currentValue, object iteratedValue)
        {
            if (currentValue is int intCurrent && iteratedValue is int intIterated)
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

        public override bool Validate(object item)
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

        public override GUIContent[] ResolveGroupedName(object value, DisplayGrouping grouping)
        {
            return new GUIContent[] { new GUIContent(SelectUtility.NotSupported) };
        }

        public override List<object> Setup()
        {
            _enumType = GetFieldOrElementType();
            _isFlag = _enumType.GetCustomAttribute<FlagsAttribute>() != null;

            var ints = EnumUtility.GetAllValues(_enumType).Select(x => x.ToFlagInt()).ToList();
            if (_isFlag)
            {
                _none = new PredefinedValues(Constants.NoneEnumName, EnumUtility.DefaultIntFlag);
                if (!ints.Contains(_none.Value))
                {
                    ints.Insert(0, _none.Value);
                }

                var everything = EnumUtility.EverythingFlag(_enumType).ToFlagInt();
                _everythingValue = new PredefinedValues(Constants.EverythingEnumName, everything);
                if (!ints.Contains(_everythingValue.Value))
                {
                    ints.Insert(ints.Count, _everythingValue.Value);
                }
            }

            return ints.Cast<object>().ToList();
        }

        public override GUIContent ResolveName(object value, DisplayName displayName)
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

            return new GUIContent(SelectUtility.NotSupported);
        }
    }
}