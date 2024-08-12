using System;
using Better.Attributes.Runtime.Manipulation;
using Better.Commons.EditorAddons.Extensions;
using Better.Internal.Core.Runtime;
using UnityEditor;

namespace Better.Attributes.EditorAddons.Drawers.Manipulation
{
    public class ManipulateUserConditionHandler : ManipulateHandler
    {
        private ManipulateUserConditionAttribute _userAttribute;

        private object _container;

        public override void Deconstruct()
        {
        }

        protected override bool IsConditionSatisfied()
        {
            if (_container == null) return false;
            var type = _container.GetType();
            var field = type.GetField(_userAttribute.MemberName, Defines.FieldsFlags);
            var memberValue = _userAttribute.MemberValue;
            if (field != null)
            {
                var value = field.GetValue(_container);
                return Equals(memberValue, value);
            }

            var method = type.GetMethod(_userAttribute.MemberName, Defines.MethodFlags);
            if (method != null)
            {
                return Equals(memberValue, method.Invoke(_container, Array.Empty<object>()));
            }
            
            var property = type.GetProperty(_userAttribute.MemberName, Defines.FieldsFlags);
            var propertyValue = _userAttribute.MemberValue;
            if (property != null)
            {
                var value = property.GetValue(_container);
                return Equals(memberValue, propertyValue);
            }

            return false;
        }

        public override void SetProperty(SerializedProperty property, ManipulateAttribute attribute)
        {
            base.SetProperty(property, attribute);
            _userAttribute = (ManipulateUserConditionAttribute)attribute;
            _container = _property.GetLastNonCollectionContainer();
        }
    }
}