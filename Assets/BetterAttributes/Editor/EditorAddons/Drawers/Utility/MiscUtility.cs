using System;
using System.Collections.Generic;
using Better.Attributes.EditorAddons.Comparers;
using Better.Attributes.EditorAddons.Drawers.Misc;
using Better.Attributes.Runtime.Misc;
using Better.Attributes.Runtime.Rename;
using Better.Commons.EditorAddons.Drawers.Utility;
using Better.Commons.EditorAddons.Drawers.WrappersTypeCollection;
using Better.Commons.Runtime.Comparers;

namespace Better.Attributes.EditorAddons.Drawers.Utility
{
    public class MiscUtility : SerializedPropertyUtility<MiscUtility>
    {
        protected override BaseWrappersTypeCollection GenerateCollection()
        {
            return new WrappersTypeCollection(TypeComparer.Instance)
            {
                {
                    typeof(HideLabelAttribute), new Dictionary<Type, Type>(AnyTypeComparer.Instance)
                    {
                        { typeof(Type), typeof(HideLabelHandler) }
                    }
                },
                {
                    typeof(EnumButtonsAttribute), new Dictionary<Type, Type>(AssignableFromComparer.Instance)
                    {
                        { typeof(Enum), typeof(EnumButtonsHandler) }
                    }
                },
                {
                    typeof(CustomTooltipAttribute), new Dictionary<Type, Type>(AnyTypeComparer.Instance)
                    {
                        { typeof(Type), typeof(CustomToolTipHandler) }
                    }
                },
                {
                    typeof(HelpBoxAttribute), new Dictionary<Type, Type>(AnyTypeComparer.Instance)
                    {
                        { typeof(Type), typeof(HelpBoxHandler) }
                    }
                },
                {
                    typeof(RenameFieldAttribute), new Dictionary<Type, Type>(AnyTypeComparer.Instance)
                    {
                        { typeof(Type), typeof(HelpBoxHandler) }
                    }
                },
            };
        }

        protected override HashSet<Type> GenerateAvailable()
        {
            return new HashSet<Type>(AnyTypeComparer.Instance)
            {
                typeof(Enum),
                typeof(Type)
            };
        }
    }
}