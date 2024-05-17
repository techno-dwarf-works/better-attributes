using System;
using System.Collections.Generic;
using Better.Attributes.EditorAddons.Comparers;
using Better.Attributes.EditorAddons.Drawers.Misc.Wrappers;
using Better.Attributes.Runtime.Misc;
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
                        { typeof(Type), typeof(HideLabelWrapper) }
                    }
                },
                {
                    typeof(EnumButtonsAttribute), new Dictionary<Type, Type>(AssignableFromComparer.Instance)
                    {
                        { typeof(Enum), typeof(EnumButtonsWrapper) }
                    }
                },
                {
                    typeof(CustomTooltipAttribute), new Dictionary<Type, Type>(AnyTypeComparer.Instance)
                    {
                        { typeof(Type), typeof(CustomToolTipWrapper) }
                    }
                },
                {
                    typeof(HelpBoxAttribute), new Dictionary<Type, Type>(AnyTypeComparer.Instance)
                    {
                        { typeof(Type), typeof(HelpBoxWrapper) }
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