using System;
using System.Collections.Generic;
using Better.Attributes.EditorAddons.Drawers.Manipulation;
using Better.Attributes.Runtime.Manipulation;
using Better.Commons.EditorAddons.Drawers.Utility;
using Better.Commons.EditorAddons.Drawers.WrappersTypeCollection;
using Better.Commons.Runtime.Comparers;

namespace Better.Attributes.EditorAddons.Drawers.Utility
{
    public class __ManipulateUtility : HandlerMap<__ManipulateUtility>
    {
        protected override BaseWrappersTypeCollection GenerateCollection()
        {
            return new AttributeWrappersTypeCollection(AssignableFromComparer.Instance)
            {
                { typeof(ManipulateUserConditionAttribute), typeof(ManipulateUserConditionWrapper) },
                { typeof(DisableInEditorModeAttribute), typeof(InEditorModeWrapper) },
                { typeof(DisableInPlayModeAttribute), typeof(InPlayModeWrapper) },
                { typeof(EnableInEditorModeAttribute), typeof(InEditorModeWrapper)},
                { typeof(EnableInPlayModeAttribute), typeof(InPlayModeWrapper) },
                { typeof(ShowInPlayModeAttribute), typeof(InPlayModeWrapper) },
                { typeof(ShowInEditorModeAttribute), typeof(InEditorModeWrapper) },
                { typeof(HideInPlayModeAttribute), typeof(InPlayModeWrapper) },
                { typeof(HideInEditorModeAttribute), typeof(InEditorModeWrapper) },
                { typeof(ReadOnlyAttribute), typeof(ReadOnlyFieldAttributeWrapper) },
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