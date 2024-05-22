using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Better.Commons.Runtime.Comparers;
using Better.Commons.Runtime.Utility;
using Better.Internal.Core.Runtime;
using UnityObject = UnityEngine.Object;

namespace Better.Commons.Runtime.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsArrayOrList(this Type self)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return false;
            }

            if (self.IsArray)
            {
                return true;
            }

            var listType = typeof(List<>);
            return listType.IsAssignableFromRawGeneric(self);
        }

        public static bool IsEnumerable(this Type self)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return false;
            }

            if (self.IsArray)
            {
                return true;
            }

            var enumerableType = typeof(IEnumerable<>);
            return enumerableType.IsAssignableFromRawGeneric(self);
        }

        public static bool IsAssignableFromRawGeneric(this Type self, Type type)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return false;
            }

            if (type == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(type));
                return false;
            }

            var bufferType = type;
            while (bufferType.BaseType != null)
            {
                if (bufferType.IsGeneric(self))
                {
                    return true;
                }
                
                foreach (var subType in bufferType.GetInterfaces())
                {
                    if (!subType.IsGeneric(self)) continue;
                    return true;
                }

                bufferType = bufferType.BaseType;
            }

            return false;
        }

        public static bool IsGeneric(this Type self, Type type)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return false;
            }

            if (self.IsGenericType && self.GetGenericTypeDefinition() == type)
            {
                return true;
            }

            return false;
        }

        public static bool IsGeneric<T>(this Type self)
        {
            var type = typeof(T);
            return self.IsGeneric(type);
        }

        public static bool IsEnum(this Type self, bool checkFlagAttribute = false)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return false;
            }

            if (self.IsEnum)
            {
                return !checkFlagAttribute || self.GetCustomAttribute<FlagsAttribute>() != null;
            }

            return false;
        }

        public static object GetDefault(this Type self)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return null;
            }

            if (self.IsValueType)
            {
                return Activator.CreateInstance(self);
            }

            if (self == typeof(string))
            {
                return string.Empty;
            }

            return null;
        }

        public static IEnumerable<Type> GetAllInheritedTypes(this Type self)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return null;
            }

            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => self.IsAssignableFrom(type) && (type.IsClass || type.IsValueType) && !type.IsAbstract);
        }

        public static IEnumerable<Type> GetAllInheritedTypes(this Type self, params Type[] excludes)
        {
            return self.GetAllInheritedTypes().Except(excludes, IsSubClassComparer.Instance);
        }

        public static IEnumerable<Type> GetAllInheritedTypesWithoutUnityObject(this Type self)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return null;
            }

            var unityObjectType = typeof(UnityObject);
            if (self.IsSubclassOf(unityObjectType))
            {
                return Enumerable.Empty<Type>();
            }

            return self.GetAllInheritedTypes(unityObjectType);
        }

        public static bool IsAnonymous(this Type type)
        {
            if (type.IsClass && type.IsSealed && type.Attributes.HasFlag(TypeAttributes.NotPublic))
            {
                var attributes = type.GetCustomAttribute<CompilerGeneratedAttribute>(false);
                if (attributes != null)
                {
                    return true;
                }
            }

            return false;
        }

        public static IEnumerable<Type> GetAllInheritedTypesOfRawGeneric(this Type self)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return null;
            }

            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsSubclassOfRawGeneric(self));
        }

        public static IEnumerable<Type> GetAllInheritedTypesOfRawGeneric(this Type self, params Type[] excludes)
        {
            return self.GetAllInheritedTypesOfRawGeneric().Except(excludes);
        }

        public static bool IsSubclassOf<T>(this Type self)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return false;
            }

            var type = typeof(T);
            return self.IsSubclassOf(type);
        }

        public static bool IsSubclassOfAny(this Type self, IEnumerable<Type> anyTypes)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return false;
            }

            if (anyTypes == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(anyTypes));
                return false;
            }

            foreach (var anyType in anyTypes)
            {
                if (anyType != null && self.IsSubclassOf(anyType))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsSubclassOfRawGeneric(this Type self, Type genericType)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return false;
            }

            if (genericType == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(genericType));
                return false;
            }

            while (self != null && self != typeof(object))
            {
                var definition = self.IsGenericType ? self.GetGenericTypeDefinition() : self;
                if (genericType == definition && self != genericType)
                {
                    return true;
                }

                self = self.BaseType;
            }

            return false;
        }

        public static bool IsSubclassOfAnyRawGeneric(this Type self, IEnumerable<Type> genericTypes)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return false;
            }

            if (genericTypes == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(genericTypes));
                return false;
            }

            foreach (var anyType in genericTypes)
            {
                if (anyType != null && self.IsSubclassOfRawGeneric(anyType))
                {
                    return true;
                }
            }

            return false;
        }

        public static Type GetCollectionElementType(this Type self)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return null;
            }

            if (self.IsArray)
            {
                return self.GetElementType();
            }

            if (self.IsEnumerable())
            {
                return self.GetGenericArguments()[0];
            }

            return null;
        }

        public static bool IsNullable(this Type self)
        {
            if (self == null || !self.IsValueType)
            {
                return true;
            }

            return Nullable.GetUnderlyingType(self) != null;
        }

        public static IEnumerable<MemberInfo> GetMembersRecursive(this Type type)
        {
            if (type == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(type));
                return Enumerable.Empty<MemberInfo>();
            }

            var members = new HashSet<MemberInfo>(MemberInfoComparer.Instance);

            const BindingFlags methodFlags = Defines.MethodFlags & ~BindingFlags.DeclaredOnly;
            do
            {
                // If the type is a constructed generic type, get the members of the generic type definition
                var typeToReflect = type.IsGenericType && !type.IsGenericTypeDefinition ? type.GetGenericTypeDefinition() : type;

                foreach (var member in typeToReflect.GetMembers(methodFlags))
                {
                    // For generic classes, convert members back to the constructed type
                    var memberToAdd = type.IsGenericType && !type.IsGenericTypeDefinition
                        ? ConvertToConstructedGenericType(member, type)
                        : member;

                    if (memberToAdd != null)
                    {
                        members.Add(memberToAdd);
                    }
                }

                type = type.BaseType;
            } while (type != null); // Continue until you reach the top of the inheritance hierarchy

            return members;
        }

        private static MemberInfo ConvertToConstructedGenericType(MemberInfo memberInfo, Type constructedType)
        {
            // Ensure the member's declaring type is a generic type definition
            if (memberInfo.DeclaringType != null && memberInfo.DeclaringType.IsGenericTypeDefinition)
            {
                var members = constructedType.GetMember(memberInfo.Name);
                return members.FirstOrDefault();
            }

            // Return the original memberInfo if it's not a property of a generic type definition or doesn't need to be constructed
            return memberInfo;
        }

        public static MemberInfo GetMemberByNameRecursive(this Type type, string memberName)
        {
            if (type == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(type));
                return null;
            }

            if (string.IsNullOrEmpty(memberName))
            {
                DebugUtility.LogException<ArgumentException>(nameof(memberName));
                return null;
            }

            var allMembers = GetMembersRecursive(type);

            // Use LINQ to find the member by name. This assumes you want the first match if there are multiple members with the same name (overloads).
            // If you expect overloads and want to handle them differently, you might need a more complex approach.
            return allMembers.FirstOrDefault(m => m.Name == memberName);
        }
    }
}