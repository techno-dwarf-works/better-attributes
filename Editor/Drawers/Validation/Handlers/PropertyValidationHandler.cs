using System;
using System.Reflection;
using Better.Attributes.Runtime.Validation;
using UnityEditor;

namespace Better.Attributes.EditorAddons.Drawers.Validation.Handlers
{
    [Serializable]
    public abstract class PropertyValidationHandler : ValidationHandler
    {
        protected internal SerializedProperty Property { get; private set; }
        protected internal ValidationAttribute Attribute { get; private set; }
        protected internal FieldInfo FieldInfo { get; private set; }

        public override ValidationType Type => Attribute.ValidationType;

        public void Setup(SerializedProperty property, FieldInfo fieldInfo, ValidationAttribute attribute)
        {
            Property = property;
            Attribute = attribute;
            FieldInfo = fieldInfo;

            OnSetup();
        }

        protected virtual void OnSetup()
        {
        }

        public override void Deconstruct()
        {
            Property = null;
        }
    }
}