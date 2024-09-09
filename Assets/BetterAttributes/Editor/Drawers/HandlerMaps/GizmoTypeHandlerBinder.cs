using System;
using System.Collections.Generic;
using Better.Attributes.EditorAddons.Drawers.Gizmo;
using Better.Attributes.Runtime.Gizmo;
using Better.Commons.EditorAddons.Drawers.Handlers;
using Better.Commons.EditorAddons.Drawers.HandlersTypeCollection;
using UnityEngine;

namespace Better.Attributes.EditorAddons.Drawers.HandlerMaps
{
    [Binder(typeof(GizmoHandler))]
    public class GizmoTypeHandlerBinder : TypeHandlerBinder<GizmoHandler>
    {
        protected override BaseHandlersTypeCollection GenerateCollection()
        {
            return new HandlersTypeCollection()
            {
                {
                    typeof(GizmoLocalAttribute), new Dictionary<Type, Type>()
                    {
                        { typeof(Vector3), typeof(Vector3LocalHandler) },
                        { typeof(Vector2), typeof(Vector2LocalHandler) },
                        { typeof(Quaternion), typeof(QuaternionLocalHandler) },
                        { typeof(Bounds), typeof(BoundsLocalHandler) }
                    }
                },
                {
                    typeof(GizmoAttribute), new Dictionary<Type, Type>()
                    {
                        { typeof(Vector3), typeof(Vector3Handler) },
                        { typeof(Vector2), typeof(Vector2Handler) },
                        { typeof(Quaternion), typeof(QuaternionHandler) },
                        { typeof(Bounds), typeof(BoundsHandler) }
                    }
                }
            };
        }

        protected override HashSet<Type> GenerateAvailable()
        {
            return new HashSet<Type>()
            {
                typeof(Vector3),
                typeof(Vector2),
                typeof(Quaternion),
                typeof(Bounds)
            };
        }
    }
}