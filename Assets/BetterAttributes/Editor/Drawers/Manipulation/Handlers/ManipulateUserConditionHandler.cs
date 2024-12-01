using System;
using System.Reflection;
using Better.Attributes.Runtime.Manipulation;
using Better.Commons.EditorAddons.Drawers;
using Better.Commons.EditorAddons.Extensions;
using Better.Commons.Runtime.Extensions;
using Better.Internal.Core.Runtime;
using UnityEditor;

namespace Better.Attributes.EditorAddons.Drawers.Manipulation
{
    [HandlerBinding(typeof(ManipulateUserConditionAttribute))]
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
            var memberInfo = type.GetMemberByNameRecursive(_userAttribute.MemberName);
            var memberValue = _userAttribute.MemberValue;
            if (memberInfo is FieldInfo fieldInfo)
            {
                var value = fieldInfo.GetValue(_container);
                return Equals(memberValue, value);
            }

            if (memberInfo is MethodInfo methodInfo)
            {
                return Equals(memberValue, methodInfo.Invoke(_container, Array.Empty<object>()));
            }
            
            if (memberInfo is PropertyInfo propertyInfo)
            {
                var value = propertyInfo.GetValue(_container);
                return Equals(memberValue, value);
            }

            return false;
        }

        public override void SetProperty(SerializedProperty property, ManipulateAttribute attribute)
        {
            base.SetProperty(property, attribute);
            _userAttribute = (ManipulateUserConditionAttribute)attribute;
            _container = _property.GetLastNonCollectionParent();
        }
    }
}