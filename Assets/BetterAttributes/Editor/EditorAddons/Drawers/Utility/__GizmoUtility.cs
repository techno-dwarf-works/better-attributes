using System;
using System.Collections.Generic;
using Better.Attributes.EditorAddons.Drawers.Gizmo;
using Better.Attributes.Runtime.Gizmo;
using Better.Commons.EditorAddons.Drawers.Utility;
using Better.Commons.EditorAddons.Drawers.WrappersTypeCollection;
using UnityEngine;

namespace Better.Attributes.EditorAddons.Drawers.Utility
{
    public class __GizmoUtility : HandlerMap<__GizmoUtility>
    {
        protected override BaseWrappersTypeCollection GenerateCollection()
        {
            return new WrappersTypeCollection()
            {
                {
                    typeof(GizmoLocalAttribute), new Dictionary<Type, Type>()
                    {
                        { typeof(Vector3), typeof(Vector3LocalWrapper) },
                        { typeof(Vector2), typeof(Vector2LocalWrapper) },
                        { typeof(Quaternion), typeof(QuaternionLocalWrapper) },
                        { typeof(Bounds), typeof(BoundsLocalWrapper) }
                    }
                },
                {
                    typeof(GizmoAttribute), new Dictionary<Type, Type>()
                    {
                        { typeof(Vector3), typeof(Vector3Wrapper) },
                        { typeof(Vector2), typeof(Vector2Wrapper) },
                        { typeof(Quaternion), typeof(QuaternionWrapper) },
                        { typeof(Bounds), typeof(BoundsWrapper) }
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