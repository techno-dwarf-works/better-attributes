using System;
using System.Collections.Generic;
using Better.Attributes.EditorAddons.Comparers;
using Better.Attributes.EditorAddons.Drawers.Select;
using Better.Attributes.Runtime.Select;
using Better.Commons.EditorAddons.Drawers.Handlers;
using Better.Commons.EditorAddons.Drawers.HandlersTypeCollection;
using Better.Commons.Runtime.Comparers;
using Better.Commons.Runtime.DataStructures.SerializedTypes;

#pragma warning disable CS0618

namespace Better.Attributes.EditorAddons.Drawers.HandlerMaps
{
    [Binder(typeof(BaseSelectHandler))]
    public class SelectTypeHandlerBinder : TypeHandlerBinder<BaseSelectHandler>
    {
        protected override BaseHandlersTypeCollection GenerateCollection()
        {
            return new HandlersTypeCollection(TypeComparer.Instance)
            {
                {
                    typeof(SelectAttribute), new Dictionary<Type, Type>(TypeComparer.Instance)
                    {
                        { typeof(SerializedType), typeof(SelectSerializedTypeHandler) },
                        { typeof(Enum), typeof(SelectEnumHandler) },
                        { typeof(Type), typeof(SelectImplementationHandler) }
                    }
                },
                {
                    typeof(DropdownAttribute), new Dictionary<Type, Type>(AnyTypeComparer.Instance)
                    {
                        { typeof(Type), typeof(DropdownHandler) }
                    }
                }
            };
        }

        protected override HashSet<Type> GenerateAvailable()
        {
            return new HashSet<Type>(AnyTypeComparer.Instance)
            {
                typeof(Enum),
                typeof(Type),
                typeof(SerializedType)
            };
        }
    }
}