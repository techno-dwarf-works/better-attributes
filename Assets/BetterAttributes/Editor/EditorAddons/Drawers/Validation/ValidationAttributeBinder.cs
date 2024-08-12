using System;
using System.Collections.Generic;
using Better.Attributes.EditorAddons.Drawers.Validation.Handlers;
using Better.Attributes.Runtime.Validation;
using Better.Commons.EditorAddons.Drawers.Handlers;
using Better.Commons.EditorAddons.Drawers.HandlersTypeCollection;
using Better.Commons.Runtime.Comparers;

namespace Better.Attributes.EditorAddons.Drawers.Validation
{
    [Binder(typeof(PropertyValidationHandler))]
    public class ValidationAttributeBinder : TypeHandlerBinder<PropertyValidationHandler>
    {
        protected override BaseHandlersTypeCollection GenerateCollection()
        {
            return new AttributeHandlersTypeCollection(AssignableFromComparer.Instance)
            {
                { typeof(NotNullAttribute), typeof(NotNullHandler) },
                { typeof(PrefabReferenceAttribute), typeof(PrefabHandler) },
                { typeof(SceneReferenceAttribute), typeof(SceneReferenceHandler) },
                { typeof(FindAttribute), typeof(FindComponentHandler) },
                { typeof(DataValidationAttribute), typeof(DataValidationHandler) },
                { typeof(MaxAttribute), typeof(MaxWrapper) },
                { typeof(ClampAttribute), typeof(ClampWrapper) },
            };
        }

        public override bool IsSupported(Type type)
        {
            return true;
        }

        protected override HashSet<Type> GenerateAvailable()
        {
            return new HashSet<Type>();
        }
    }
}