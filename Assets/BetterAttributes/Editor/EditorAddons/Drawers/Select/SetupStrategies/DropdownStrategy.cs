using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Better.Attributes.EditorAddons.Drawers.Utility;
using Better.Attributes.EditorAddons.Extensions;
using Better.Attributes.Runtime;
using Better.Attributes.Runtime.Select;
using Better.Commons.Runtime.Comparers;
using Better.Commons.Runtime.Extensions;
using Better.Commons.Runtime.Utility;
using Better.Internal.Core.Runtime;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Better.Attributes.EditorAddons.Drawers.Select.SetupStrategies
{
    public class DropdownStrategy : SetupStrategy
    {
        private HashSet<Type> _ignoreNameSplit = new HashSet<Type>()
        {
            typeof(float), typeof(decimal), typeof(double)
        };

        private readonly DropdownAttribute _dropdownAttribute;
        private IDataCollection _data = new NoneCollection();

        public DropdownStrategy(FieldInfo fieldInfo, object propertyContainer, SelectAttributeBase selectAttributeBase) : base(fieldInfo, propertyContainer,
            selectAttributeBase)
        {
            _dropdownAttribute = (DropdownAttribute)selectAttributeBase;
        }
        
        public override bool IsSkipingFieldDraw()
        {
            return true;
        }

        private bool TryGetType(IEnumerable<string> path, out Type type)
        {
            var buffer = new Stack<string>(path);
            var typeString = buffer.Aggregate(buffer.Pop(), (input, item) => input + AttributesDefinitions.Dot + item);

            type = Type.GetType(typeString, false, true);
            if (type != null) return true;
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var bufferType = assembly.GetTypes().FirstOrDefault(t => t.Name == typeString);
                if (bufferType != null)
                {
                    type = bufferType;
                    return true;
                }
            }

            return type != null;
        }

        public override List<object> Setup()
        {
            var memberName = _dropdownAttribute.SelectorName;
            var type = _propertyContainer.GetType();
            var instance = _propertyContainer;
            if (memberName.StartsWith(AttributesDefinitions.SelectorDefinition))
            {
                memberName = memberName.Replace(AttributesDefinitions.SelectorDefinition, string.Empty);
                instance = GetItemsFrom(ref memberName, out type);
            }

            var response = FetchResponse(type, memberName, instance);

            switch (response)
            {
                case null:
                    return new List<object>();
                case IDictionary dictionary:
                    _data = new DictionaryCollection(dictionary, _dropdownAttribute.ShowDefault, _dropdownAttribute.ShowUniqueKey);
                    break;
                case IList list:
                    _data = new ListCollection(list, _dropdownAttribute.ShowDefault, _dropdownAttribute.ShowUniqueKey);
                    break;
            }

            return _data.GetValues();
        }

        private object FetchResponse(Type type, string memberName, object instance)
        {
            var member = type.GetMember(memberName, Defines.MethodFlags);
            object response = null;
            foreach (var memberInfo in member)
            {
                switch (memberInfo)
                {
                    case PropertyInfo propertyInfo:
                        response = propertyInfo.GetValue(instance);
                        break;
                    case FieldInfo fieldInfo:
                        response = fieldInfo.GetValue(instance);
                        break;
                    case MethodInfo methodInfo:
                        response = methodInfo.Invoke(instance, Array.Empty<object>());
                        break;
                }

                if (response != null)
                {
                    break;
                }
            }

            return response;
        }

        private object GetItemsFrom(ref string memberName, out Type type)
        {
            var path = new Stack<string>(memberName.Split(AttributesDefinitions.NameSeparator));
            memberName = path.Pop().Replace(AttributesDefinitions.Brackets, string.Empty);
            if (TryGetType(path, out type)) return null;
            var instanceName = path.Pop().Replace(AttributesDefinitions.Brackets, string.Empty);

            if (!TryGetType(path, out type))
            {
                DebugUtility.LogException<TypeAccessException>();
                return null;
            }

            var members = type.GetMemberByNameRecursive(instanceName);

            var itemsFrom = GetInstance(members);
            return itemsFrom;
        }

        private static object GetInstance(MemberInfo memberInfo)
        {
            switch (memberInfo)
            {
                case PropertyInfo propertyInfo when propertyInfo.GetGetMethod().IsStatic:
                    return propertyInfo.GetValue(null);
                case FieldInfo { IsStatic: true } fieldInfo:
                    return fieldInfo.GetValue(null);
                case MethodInfo { IsStatic: true } methodInfo when methodInfo.GetParameters().Length <= 0:
                    return methodInfo.Invoke(null, Array.Empty<object>());
                default:
                    return null;
            }
        }

        public override GUIContent ResolveName(object value, DisplayName displayName)
        {
            if (value == null)
            {
                return new GUIContent(LabelDefines.Null);
            }

            var name = _data.FindName(value);
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

                    var split = name.Split(AttributesDefinitions.NameSeparator);
                    return split.Length <= 1 ? new GUIContent(name) : new GUIContent(split.Last());
                case DisplayName.Full:
                    return new GUIContent(name);
                default:
                    DebugUtility.LogException<ArgumentOutOfRangeException>(nameof(displayName));
                    return null;
            }
        }

        public override GUIContent[] ResolveGroupedName(object value, DisplayGrouping grouping)
        {
            if (value == null)
            {
                return new GUIContent[] { new GUIContent(LabelDefines.Null) };
            }

            var type = _data.FindName(value);

            if (_ignoreNameSplit.Contains(value.GetType()))
            {
                return new GUIContent[] { new GUIContent(type) };
            }

            var split = type.Split(AttributesDefinitions.NameSeparator);
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

        public override string GetButtonName(object currentValue)
        {
            return _data.FindName(currentValue);
        }

        public override bool ResolveState(object currentValue, object iteratedValue)
        {
            return Equals(currentValue, iteratedValue);
        }

        public override bool Validate(object item)
        {
            var type = GetFieldOrElementType();

            if (item == null)
            {
                return true;
            }

            return TypeComparer.Instance.Equals(type, item.GetType());
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