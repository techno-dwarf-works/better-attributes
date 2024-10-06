using System;
using System.Collections.Generic;
using Better.Attributes.EditorAddons.Drawers.Manipulation;
using Better.Attributes.Runtime.Manipulation;
using Better.Commons.EditorAddons.Drawers.Handlers;
using Better.Commons.EditorAddons.Drawers.HandlersTypeCollection;
using Better.Commons.Runtime.Comparers;

namespace Better.Attributes.EditorAddons.Drawers.HandlerMaps
{
    [Binder(typeof(ManipulateHandler))]
    public class ManipulateTypeHandlerBinder : TypeHandlerBinder<ManipulateHandler>
    {
        protected override BaseHandlersTypeCollection GenerateCollection()
        {
            return new AttributeHandlersTypeCollection(AssignableFromComparer.Instance)
            {
                { typeof(ManipulateUserConditionAttribute), typeof(ManipulateUserConditionHandler) },
                { typeof(DisableInEditorModeAttribute), typeof(InEditorModeHandler) },
                { typeof(DisableInPlayModeAttribute), typeof(InPlayModeHandler) },
                { typeof(EnableInEditorModeAttribute), typeof(InEditorModeHandler)},
                { typeof(EnableInPlayModeAttribute), typeof(InPlayModeHandler) },
                { typeof(ShowInPlayModeAttribute), typeof(InPlayModeHandler) },
                { typeof(ShowInEditorModeAttribute), typeof(InEditorModeHandler) },
                { typeof(HideInPlayModeAttribute), typeof(InPlayModeHandler) },
                { typeof(HideInEditorModeAttribute), typeof(InEditorModeHandler) },
                { typeof(ReadOnlyAttribute), typeof(ReadOnlyFieldAttributeHandler) },
            };
        }

        protected override HashSet<Type> GenerateAvailable()
        {
            return new HashSet<Type>();
        }

        public override bool IsSupported(Type type)
        {
            return true;
        }
    }
}