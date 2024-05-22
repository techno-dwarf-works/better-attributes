using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Better.Commons.EditorAddons.Helpers;
using Better.Commons.EditorAddons.Utility;
using Better.Commons.Runtime.Extensions;
using Better.Commons.Runtime.Utility;
using Better.Internal.Core.Runtime;
using UnityEditor;
using UnityEngine;

namespace Better.Commons.EditorAddons.Extensions
{
    public static class SerializedPropertyExtensions
    {
        private const string ScriptFieldName = "m_Script";
        private const string NativeObjectPtrName = "m_NativeObjectPtr";
        private const string NativePropertyPtrName = "m_NativePropertyPtr";
        private const string VerifyMethodName = "Verify";
        private static readonly MethodInfo VerifyMethod;
        private static readonly FieldInfo PropertyPrtInfo;
        private static readonly FieldInfo ObjectPrtInfo;

        static SerializedPropertyExtensions()
        {
            var serializedPropertyType = typeof(SerializedProperty);
            VerifyMethod = serializedPropertyType.GetMethod(VerifyMethodName, Defines.FieldsFlags);
            PropertyPrtInfo = serializedPropertyType.GetField(NativePropertyPtrName, Defines.FieldsFlags);
            ObjectPrtInfo = typeof(SerializedObject).GetField(NativeObjectPtrName, Defines.FieldsFlags);
        }

        public static Type GetManagedType(this SerializedProperty self)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return null;
            }

#if UNITY_2021_2_OR_NEWER
            return self.managedReferenceValue?.GetType();
#else
            if (string.IsNullOrEmpty(self.managedReferenceFullTypename))
            {
                return null;
            }

            var split = self.managedReferenceFullTypename.Split(' ');
            var assembly = GetAssembly(split[0]);
            var currentValue = assembly.GetType(split[1]);
            return currentValue;
#endif
        }

        private static Assembly GetAssembly(string name)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SingleOrDefault(assembly => assembly.GetName().Name == name);
        }

        public static bool IsArrayElement(this SerializedProperty self)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return false;
            }

            return self.propertyPath.EndsWith("]");
        }

        public static int GetArrayIndex(this SerializedProperty self)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return -1;
            }

            var matches = SerializedPropertyUtility.ArrayIndexRegex.Matches(self.propertyPath);
            if (matches.Count > 0)
            {
                if (int.TryParse(matches[matches.Count - 1].Name, out var result))
                {
                    return result;
                }
            }

            return -1;
        }

        public static CachedFieldInfo GetFieldInfoAndStaticTypeFromProperty(this SerializedProperty self)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return null;
            }

            var classType = GetScriptTypeFromProperty(self);
            if (classType == null)
            {
                return null;
            }

            var fieldPath = self.propertyPath;
            if (self.propertyType == SerializedPropertyType.ManagedReference)
            {
                var objectTypename = self.managedReferenceFullTypename;
                if (!SerializedPropertyUtility.GetTypeFromManagedReferenceFullTypeName(objectTypename, out classType))
                {
                    return null;
                }

                fieldPath = self.propertyPath;
            }

            if (classType == null)
            {
                return null;
            }

            return SerializedPropertyUtility.GetFieldInfoFromPropertyPath(classType, fieldPath);
        }

        public static Type GetScriptTypeFromProperty(this SerializedProperty self)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return null;
            }

            if (self.serializedObject.targetObject != null)
                return self.serializedObject.targetObject.GetType();

            // Fallback in case the targetObject has been destroyed but the property is still valid.
            var scriptProp = self.serializedObject.FindProperty(ScriptFieldName);

            if (scriptProp == null)
                return null;

            var script = scriptProp.objectReferenceValue as MonoScript;

            return script == null ? null : script.GetClass();
        }

        public static List<TAttributes> GetAttributes<TAttributes>(this SerializedProperty self, bool inherit = false) where TAttributes : Attribute
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return new List<TAttributes>();
            }

            var fieldInfo = self.GetFieldInfoAndStaticTypeFromProperty();
            if (fieldInfo == null) return null;
            var attributes = fieldInfo.FieldInfo.GetCustomAttributes<TAttributes>(inherit);
            return attributes.ToList();
        }

        public static string GetPropertyParentList(this SerializedProperty self)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return string.Empty;
            }

            string propertyPath = self.propertyPath;
            return SerializedPropertyUtility.GetPropertyParentList(propertyPath);
        }

        public static string GetArrayNameFromPath(this SerializedProperty self)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return string.Empty;
            }

            return SerializedPropertyUtility.ArrayDataWithIndexRegex.Replace(self.propertyPath, "");
        }

        public static string GetArrayPath(this SerializedProperty self)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return string.Empty;
            }

            return SerializedPropertyUtility.ArrayRegex.Replace(self.propertyPath, "");
        }

        public static bool IsDisposed(this SerializedObject self)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return true;
            }

            try
            {
                if (ObjectPrtInfo != null)
                {
                    var objectPrt = (IntPtr)ObjectPrtInfo.GetValue(self);
                    return objectPrt == IntPtr.Zero;
                }
            }
            catch
            {
                return true;
            }

            return true;
        }

        public static bool IsDisposed(this SerializedProperty self)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return true;
            }

            if (self.serializedObject == null)
            {
                return false;
            }
            
            try
            {
                if (PropertyPrtInfo != null && ObjectPrtInfo != null)
                {
                    var propertyPrt = (IntPtr)PropertyPrtInfo.GetValue(self);
                    var objectPrt = (IntPtr)ObjectPrtInfo.GetValue(self.serializedObject);
                    return propertyPrt == IntPtr.Zero || objectPrt == IntPtr.Zero;
                }
            }
            catch
            {
                return true;
            }

            return true;
        }

        public static bool Verify(this SerializedProperty self)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return false;
            }

            if (self.serializedObject == null)
            {
                return false;
            }


            try
            {
                if (VerifyMethod != null)
                {
                    VerifyMethod.Invoke(self, new object[] { SerializedPropertyUtility.IteratorNotAtEnd });
                    return true;
                }
            }
            catch
            {
                return false;
            }

            return false;
        }

        public static bool IsTargetComponent(this SerializedProperty self, out Component component)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                component = null;
                return false;
            }

            if (self.serializedObject.targetObject is Component inner)
            {
                component = inner;
                return true;
            }

            component = null;
            return false;
        }

        //https://gist.github.com/aholkner/214628a05b15f0bb169660945ac7923b

        public static object GetValue(this SerializedProperty self)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return null;
            }

            string propertyPath = self.propertyPath;
            object value = self.serializedObject.targetObject;
            int i = 0;
            while (NextPathComponent(propertyPath, ref i, out var token))
                value = GetPathComponentValue(value, token);
            return value;
        }

        public static void SetValue(this SerializedProperty self, object value)
        {
            Undo.RecordObject(self.serializedObject.targetObject, $"Set {self.name}");

            SetValueNoRecord(self, value);

            EditorUtility.SetDirty(self.serializedObject.targetObject);
            self.serializedObject.ApplyModifiedProperties();
        }

        public static void SetValueNoRecord(this SerializedProperty self, object value)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return;
            }

            var container = GetPropertyContainer(self, out var deferredToken);

            SetPathComponentValue(container, deferredToken, value);
        }

        public static object GetPropertyContainer(this SerializedProperty self)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return null;
            }

            return GetPropertyContainer(self, out _);
        }

        public static object GetLastNonCollectionContainer(this SerializedProperty self)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return null;
            }

            var containers = self.GetPropertyContainers();
            for (var index = containers.Count - 1; index >= 0; index--)
            {
                var container = containers[index];
                if (container.GetType().IsEnumerable()) continue;
                return container;
            }

            return containers.FirstOrDefault();
        }

        public static List<object> GetPropertyContainers(this SerializedProperty self)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return new List<object>();
            }

            string propertyPath = self.propertyPath;
            object container = self.serializedObject.targetObject;

            int i = 0;
            PropertyItemInfo deferredToken;
            var list = new List<object>();
            list.Add(container);
            NextPathComponent(propertyPath, ref i, out deferredToken);
            while (NextPathComponent(propertyPath, ref i, out var token))
            {
                container = GetPathComponentValue(container, deferredToken);
                deferredToken = token;
                list.Add(container);
            }

            if (container.GetType().IsValueType)
            {
                var message =
                    $"Cannot use SerializedObject.SetValue on a struct object, as the result will be set on a temporary. Either change {container.GetType().Name} to a class, or use SetValue with a parent member.";
                Debug.LogWarning(message);
            }

            return list;
        }

        private static object GetPropertyContainer(SerializedProperty self, out PropertyItemInfo deferredToken)
        {
            string propertyPath = self.propertyPath;
            object container = self.serializedObject.targetObject;

            int i = 0;
            NextPathComponent(propertyPath, ref i, out deferredToken);
            while (NextPathComponent(propertyPath, ref i, out var token))
            {
                container = GetPathComponentValue(container, deferredToken);
                deferredToken = token;
            }

            return container;
        }

        private static bool NextPathComponent(string propertyPath, ref int index, out PropertyItemInfo component)
        {
            component = new PropertyItemInfo();

            if (index >= propertyPath.Length)
                return false;

            var arrayElementMatch = SerializedPropertyUtility.ArrayElementRegex.Match(propertyPath, index);
            if (arrayElementMatch.Success)
            {
                index += arrayElementMatch.Length + 1; // Skip past next '.'
                component.ElementIndex = int.Parse(arrayElementMatch.Groups[1].Value);
                return true;
            }

            int dot = propertyPath.IndexOf('.', index);
            if (dot == -1)
            {
                component.PropertyName = propertyPath.Substring(index);
                index = propertyPath.Length;
            }
            else
            {
                component.PropertyName = propertyPath.Substring(index, dot - index);
                index = dot + 1; // Skip past next '.'
            }

            return true;
        }

        private static object GetPathComponentValue(object container, PropertyItemInfo component)
        {
            if (component.PropertyName == null)
                return ((IList)container)[component.ElementIndex];

            return GetMemberValue(container, component.PropertyName);
        }

        private static void SetPathComponentValue(object container, PropertyItemInfo component, object value)
        {
            if (component.PropertyName == null)
                ((IList)container)[component.ElementIndex] = value;
            else
                SetMemberValue(container, component.PropertyName, value);
        }

        private static object GetMemberValue(object container, string name)
        {
            if (container == null)
                return null;
            var type = container.GetType();
            var members = TraverseBaseClasses(type, name);
            for (int i = 0; i < members.Count; ++i)
            {
                if (members[i] is FieldInfo field)
                    return field.GetValue(container);

                if (members[i] is PropertyInfo property)
                    return property.GetValue(container);
            }

            return null;
        }

        private static List<MemberInfo> TraverseBaseClasses(Type currentType, string name)
        {
            var memberInfos = new List<MemberInfo>();

            var currentTypeFields = currentType.GetMember(name, Defines.FieldsFlags);
            memberInfos.AddRange(currentTypeFields);

            var baseType = currentType.BaseType;

            if (baseType == null) return memberInfos;
            var baseTypeFields = baseType.GetMember(name, Defines.FieldsFlags);
            memberInfos.AddRange(baseTypeFields);

            memberInfos.AddRange(TraverseBaseClasses(baseType, name)); // Recursively traverse the base classes

            return memberInfos;
        }

        private static void SetMemberValue(object container, string name, object value)
        {
            var type = container.GetType();
            var members = type.GetMember(name, Defines.FieldsFlags);
            for (int i = 0; i < members.Length; ++i)
            {
                if (members[i] is FieldInfo field)
                {
                    if (!type.IsValueType && !type.IsEnum)
                    {
                        field.SetValue(container, value);
                    }
                    else
                    {
                        field.SetValueDirect(__makeref(container), value);
                    }

                    return;
                }

                if (members[i] is PropertyInfo property)
                {
                    if (!type.IsValueType && !type.IsEnum)
                    {
                        property.SetValue(container, value);
                    }

                    return;
                }
            }

            var message = $"Failed to set member {container}.{name} via reflection";
            Debug.LogWarning(message);
        }
    }
}