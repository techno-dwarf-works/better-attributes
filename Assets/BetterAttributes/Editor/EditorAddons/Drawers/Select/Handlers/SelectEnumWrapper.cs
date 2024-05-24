using System;
using System.Reflection;
using Better.Attributes.EditorAddons.Drawers.Select.SetupStrategies;
using Better.Attributes.EditorAddons.Extensions;
using Better.Commons.EditorAddons.Extensions;
using Better.Commons.Runtime.Drawers.Attributes;
using Better.Commons.Runtime.Extensions;
using Better.Commons.Runtime.Utility;
using UnityEditor;

namespace Better.Attributes.EditorAddons.Drawers.Select
{
    public class SelectEnumWrapper : BaseSelectWrapper
    {
        private bool _isFlag;
        private int _everythingValue;

        protected override void OnSetup()
        {
            var enumType = _fieldInfo.FieldType;
            if (enumType.IsArrayOrList())
            {
                enumType = enumType.GetCollectionElementType();
            }

            _everythingValue = EnumUtility.EverythingFlag(enumType).ToFlagInt();
            _isFlag = _fieldInfo.FieldType.GetCustomAttribute<FlagsAttribute>() != null;
        }

        public override void Update(object objValue)
        {
            if (!_property.Verify()) return;
            var value = (int)objValue;
            var currentValue = _property.intValue;
            currentValue = EnumCalculator.CalculateCurrentValue(currentValue, _isFlag, value, _everythingValue);

            _property.intValue = currentValue;
        }

        public override object GetCurrentValue()
        {
            return _property.intValue;
        }
    }
}