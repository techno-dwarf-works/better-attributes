using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Better.Internal.Core.Runtime;

namespace Better.Attributes.Runtime
{
    public static class EditorButtonUtility
    {
        public static Dictionary<int, IEnumerable<KeyValuePair<MethodInfo, EditorButtonAttribute>>> GetSortedMethodAttributes(Type type)
        {
            var methodButtonsAttributes =
                new Dictionary<int, IEnumerable<KeyValuePair<MethodInfo, EditorButtonAttribute>>>();

            foreach (var pair in GetMethodsAttributes<EditorButtonAttribute>(type))
            {
                foreach (var attribute in pair.Value)
                {
                    if (methodButtonsAttributes.ContainsKey(attribute.CaptureGroup))
                    {
                        var list = methodButtonsAttributes[attribute.CaptureGroup];
                        list = list.Append(new KeyValuePair<MethodInfo, EditorButtonAttribute>(pair.Key, attribute));
                        methodButtonsAttributes[attribute.CaptureGroup] = list.OrderBy(x => x.Value.Priority);
                    }
                    else
                    {
                        methodButtonsAttributes.Add(attribute.CaptureGroup,
                            new List<KeyValuePair<MethodInfo, EditorButtonAttribute>>
                            {
                                new KeyValuePair<MethodInfo, EditorButtonAttribute>(pair.Key, attribute)
                            });
                    }
                }
            }

            return methodButtonsAttributes.OrderBy(x => x.Key).ToDictionary(x => x.Key, y => y.Value);
        }

        public static IEnumerable<KeyValuePair<MethodInfo, IEnumerable<T>>> GetMethodsAttributes<T>(Type type) where T : Attribute
        {
            return type == null
                ? Enumerable.Empty<KeyValuePair<MethodInfo, IEnumerable<T>>>()
                : type.GetMethods(Defines.MethodFlags).Where(x => x.GetCustomAttributes<T>().Any())
                    .ToDictionary(key => key, value => value.GetCustomAttributes<T>(true))
                    .Concat(GetMethodsAttributes<T>(type.BaseType));
        }
    }
}