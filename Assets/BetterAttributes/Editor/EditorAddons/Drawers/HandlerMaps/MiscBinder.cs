using System;
using System.Collections.Generic;
using Better.Attributes.EditorAddons.Comparers;
using Better.Attributes.EditorAddons.Drawers.Misc;
using Better.Attributes.Runtime.Misc;
using Better.Commons.EditorAddons.Drawers.Handlers;
using Better.Commons.EditorAddons.Drawers.HandlersTypeCollection;
using Better.Commons.Runtime.Comparers;

namespace Better.Attributes.EditorAddons.Drawers.HandlerMaps
{
    [Binder(typeof(MiscHandler))]
    public class MiscBinder : TypeHandlerBinder<MiscHandler>
    {
        protected override BaseHandlersTypeCollection GenerateCollection()
        {
            return new HandlersTypeCollection(TypeComparer.Instance)
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
                        { typeof(Type), typeof(RenameFieldHandler) }
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