using System;
using Better.Attributes.Runtime.Validation;
using Better.Commons.EditorAddons.Drawers;
using Better.Commons.EditorAddons.Extensions;
using UnityEditor;

namespace Better.Attributes.EditorAddons.Drawers.Validation.Handlers
{
    [Serializable]
    [HandlerBinding(typeof(MaxAttribute))]
    public class MaxWrapper : PropertyValidationHandler
    {
        public override bool IsSupported()
        {
            return Property.propertyType is SerializedPropertyType.Integer or SerializedPropertyType.Float;
        }

        public override ValidationValue<string> Validate()
        {
            var maxAttribute = (MaxAttribute)Attribute;
            var maxValue = maxAttribute.Max;
            bool isValid;
            switch (Property.propertyType)
            {
                case SerializedPropertyType.Float:
                    isValid = Property.floatValue <= maxValue;
                    break;
                case SerializedPropertyType.Integer:
                    isValid = Property.intValue <= maxValue;
                    break;
                default:
                    return GetNotValidValue($"Property: {Property.displayName} has invalid type: {Property.propertyType}");
            }

            if (isValid)
            {
                return GetClearValue();
            }

            switch (Property.propertyType)
            {
                case SerializedPropertyType.Float:
                    Property.SetValue(maxValue);
                    break;
                case SerializedPropertyType.Integer:
                    Property.SetValue((int)maxValue);
                    break;
            }

            EditorUtility.SetDirty(Property.serializedObject.targetObject);
            Property.serializedObject.ApplyModifiedProperties();
            return GetClearValue();
        }
    }
}