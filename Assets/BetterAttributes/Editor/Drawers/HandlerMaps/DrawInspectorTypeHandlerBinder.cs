using System;
using System.Collections.Generic;
using Better.Attributes.EditorAddons.Drawers.DrawInspector;
using Better.Attributes.Runtime.DrawInspector;
using Better.Commons.EditorAddons.Drawers.Handlers;
using Better.Commons.EditorAddons.Drawers.HandlersTypeCollection;
using Better.Commons.Runtime.Comparers;
using Object = UnityEngine.Object;

namespace Better.Attributes.EditorAddons.Drawers.HandlerMaps
{
    [Binder(typeof(DrawInspectorHandler))]
    public class DrawInspectorTypeHandlerBinder : TypeHandlerBinder<DrawInspectorHandler>
    {
        protected override BaseHandlersTypeCollection GenerateCollection()
        {
            return new HandlersTypeCollection()
            {
                {
                    typeof(DrawInspectorAttribute), new Dictionary<Type, Type>(AssignableFromComparer.Instance)
                    {
                        {
                            typeof(Object), typeof(DrawInspectorHandler)
                        }
                    }
                }
            };
        }

        protected override HashSet<Type> GenerateAvailable()
        {
            return new HashSet<Type>(AssignableFromComparer.Instance) { typeof(Object) };
        }
    }
}