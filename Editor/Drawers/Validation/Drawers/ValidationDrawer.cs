using Better.Attributes.EditorAddons.Drawers.Validation.Extensions;
using Better.Attributes.EditorAddons.Drawers.Validation.Handlers;
using Better.Attributes.Runtime.Validation;
using Better.Commons.EditorAddons.Drawers;
using Better.Commons.EditorAddons.Drawers.Container;
using Better.Commons.EditorAddons.Extensions;
using Better.Commons.Runtime.Extensions;
using UnityEditor;

namespace Better.Attributes.EditorAddons.Drawers.Validation.Drawers
{
    [CustomPropertyDrawer(typeof(ValidationAttribute), true)]
    public class ValidationDrawer : PropertyDrawer<PropertyValidationHandler, ValidationAttribute>
    {
        protected override void PopulateContainer(ElementsContainer container)
        {
            UpdateDrawer(container);
            container.SerializedPropertyChanged += UpdateDrawer;
        }

        private void UpdateDrawer(ElementsContainer container)
        {
            var handler = GetHandler(container.SerializedProperty);
            handler.Setup(container.SerializedProperty, FieldInfo, Attribute);

            if (handler.IsSupported())
            {
                var validation = handler.Validate();

                var helpBox = container.GetOrAddHelpBox(validation.Result, Attribute, handler.Type.GetMessageType());

                helpBox.style.SetVisible(!validation.State);
            }
        }
    }
}