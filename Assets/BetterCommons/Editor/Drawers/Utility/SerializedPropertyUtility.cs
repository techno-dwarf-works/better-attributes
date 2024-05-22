using System;
using System.Collections.Generic;
using Better.Commons.EditorAddons.Drawers.Base;
using Better.Commons.EditorAddons.Drawers.WrappersTypeCollection;
using Better.Commons.Runtime.Utility;
using UnityEditor;

namespace Better.Commons.EditorAddons.Drawers.Utility
{
    public abstract class SerializedPropertyUtility<THandler> where THandler : new()
    {
        private static THandler _instance;

        protected HashSet<Type> _availableTypes;
        protected BaseWrappersTypeCollection _wrappersCollection;

        public static THandler Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new THandler();
                }

                return _instance;
            }
        }

        protected SerializedPropertyUtility()
        {
            Construct();
        }

        private void Construct()
        {
            _wrappersCollection = GenerateCollection();
            _availableTypes = GenerateAvailable();
        }


        /// <summary>
        /// Type collection for <see cref="GetUtilityWrapper"/>.
        /// <example> Example:
        /// <code>
        /// return new WrappersTypeCollection()
        /// {
        ///     {
        ///         typeof(SupportedAttributeType), new ()
        ///         {
        ///             { typeof(SupportedType), typeof(WrapperForSupportedType) },
        ///             { typeof(SupportedType2), typeof(WrapperForSupportedType2) }
        ///         }
        ///     }
        /// };
        /// </code>
        /// </example>
        /// <seealso cref="GenerateAvailable"/>
        /// </summary>
        /// <returns></returns>
        protected abstract BaseWrappersTypeCollection GenerateCollection();

        /// <summary>
        /// Types collection for <see cref="IsSupported"/> checks
        /// <example> Example:
        /// <code>
        ///  return new HashSet(BaseComparer.Instance)
        ///  {
        ///     typeof(SupportedType),
        ///     typeof(SupportedType2)
        ///  };
        /// </code>
        /// </example>
        /// <seealso cref="GenerateCollection"/>
        /// </summary>
        /// <returns></returns>
        protected abstract HashSet<Type> GenerateAvailable();

        /// <summary>
        /// Generate ready to use wrapper's instance by dictionary from <see cref="GenerateCollection"/>
        /// </summary>
        /// <param name="fieldType"></param>
        /// <param name="attributeType"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetUtilityWrapper<T>(Type fieldType, Type attributeType) where T : SerializedPropertyHandler
        {
            if (fieldType == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(fieldType));
                return null;
            }

            if (attributeType == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(attributeType));
                return null;
            }

            if (!IsSupported(fieldType))
            {
                return null;
            }

            if (_wrappersCollection.TryGetValue(attributeType, fieldType, out var wrapperType))
            {
                return (T)Activator.CreateInstance(wrapperType);
            }

            DebugUtility.LogException<KeyNotFoundException>($"Supported types not found for {fieldType}");
            return null;
        }

        /// <summary>
        /// Checks if type available in utility
        /// <seealso cref="GenerateAvailable"/>
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual bool IsSupported(Type type)
        {
            if (type == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(type));
            }

            return _availableTypes.Contains(type);
        }
        
        /// <summary>
        /// Validates stored properties if their <see cref="CollectionValue{T}.Type"/> supported
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="gizmoHandlers"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="THandler"></typeparam>
        public void ValidateCachedProperties<T>(HandlerCollection<T> gizmoHandlers) where T : SerializedPropertyHandler
        {
            List<SerializedProperty> keysToRemove = null;
            foreach (var value in gizmoHandlers)
            {
                if (IsSupported(value.Value.Type)) continue;
                if (keysToRemove == null)
                {
                    keysToRemove = new List<SerializedProperty>();
                }
                keysToRemove.Add(value.Key);
            }

            if (keysToRemove != null)
            {
                foreach (var serializedProperty in keysToRemove)
                {
                    gizmoHandlers.Remove(serializedProperty);
                }
            }
        }
    }
}