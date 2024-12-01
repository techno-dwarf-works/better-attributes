using System;
using Better.Attributes.Runtime.Validation;
using Better.Commons.EditorAddons.Drawers;
using Better.Commons.Runtime.Extensions;
using UnityEditor;

namespace Better.Attributes.EditorAddons.Drawers.Validation.Handlers
{
    [Serializable]
    [HandlerBinding(typeof(NotNullAttribute))]
    public class NotNullHandler : PropertyValidationHandler
    {
        public override ValidationValue<string> Validate()
        {
            var fieldName = $"\"{Property.displayName.FormatBoldItalic()}\"";
            if (Property.propertyType == SerializedPropertyType.ObjectReference && Property.objectReferenceValue.IsNullOrDestroyed())
            {
                if (Property.objectReferenceInstanceIDValue != 0)
                {
                    return GetNotValidValue($"Object in {fieldName} field is missing reference");
                }

                return GetNotValidValue($"Object in {fieldName} field is null");
            }

            if (Property.propertyType == SerializedPropertyType.ManagedReference && Property.managedReferenceValue == default)
            {
                return GetNotValidValue($"Object in {fieldName} field is null");
            }

            return GetClearValue();
        }

        public override bool IsSupported()
        {
            var propertyType = Property.propertyType;
            return propertyType == SerializedPropertyType.ObjectReference || propertyType == SerializedPropertyType.ManagedReference;
        }
    }
}