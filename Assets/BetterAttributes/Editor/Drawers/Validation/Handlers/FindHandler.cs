using System;
using Better.Attributes.Runtime.Validation;
using Better.Commons.EditorAddons.Drawers;
using Better.Commons.Runtime.Extensions;
using UnityEditor;
using UnityEngine;

namespace Better.Attributes.EditorAddons.Drawers.Validation.Handlers
{
    [Serializable]
    [HandlerBinding(typeof(FindAttribute))]
    public class FindHandler : PropertyValidationHandler
    {
        private FindAttribute _findAttribute;

        protected override void OnSetup()
        {
            _findAttribute = (FindAttribute)Attribute;
        }
        
        public override ValidationValue<string> Validate()
        {
            var obj = Property.objectReferenceValue;
            if (_findAttribute.ValidateIfFieldEmpty)
            {
                if (obj)
                {
                    return GetClearValue();
                }
            }

            var propertySerializedObject = Property.serializedObject;
            var targetObject = propertySerializedObject.targetObject;
            var gameObject = ((Component)targetObject)?.gameObject;
            var requiredType = GetFieldOrElementType();
            if (gameObject)
            {
                switch (_findAttribute.RequireDirection)
                {
                    case RequireDirection.Parent:
                        obj = gameObject.GetComponentInParent(requiredType);
                        break;
                    case RequireDirection.None:
                        obj = gameObject.GetComponent(requiredType);
                        break;
                    case RequireDirection.Child:
                        obj = gameObject.GetComponentInChildren(requiredType);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (!obj)
            {
                return GetNotValidValue($"Reference of \"{requiredType.Name.FormatBoldItalic()}\" not found");
            }

            EditorUtility.SetDirty(targetObject);
            Property.objectReferenceValue = obj;
            propertySerializedObject.ApplyModifiedProperties();
            return GetClearValue();
        }
        
        protected Type GetFieldOrElementType()
        {
            var t = _findAttribute.RequiredType;
            if (t != null)
            {
                return t;
            }

            var fieldType = FieldInfo.FieldType;
            if (fieldType.IsArrayOrList())
                return fieldType.GetCollectionElementType();
            return fieldType;
        }

        public override bool IsSupported()
        {
            return Property.propertyType == SerializedPropertyType.ObjectReference;
        }
    }
}