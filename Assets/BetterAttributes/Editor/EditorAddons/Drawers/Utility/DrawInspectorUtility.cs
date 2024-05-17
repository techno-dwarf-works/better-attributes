using System;
using System.Collections.Generic;
using Better.Attributes.EditorAddons.Drawers.DrawInspector;
using Better.Attributes.Runtime.DrawInspector;
using Better.Commons.EditorAddons.Drawers.Utility;
using Better.Commons.EditorAddons.Drawers.WrappersTypeCollection;
using Better.Commons.Runtime.Comparers;
using Object = UnityEngine.Object;

namespace Better.Attributes.EditorAddons.Drawers.Utility
{
    public class DrawInspectorUtility : SerializedPropertyUtility<DrawInspectorUtility>
    {
        protected override BaseWrappersTypeCollection GenerateCollection()
        {
            return new WrappersTypeCollection()
            {
                {
                    typeof(DrawInspectorAttribute),
                    new Dictionary<Type, Type>(AssignableFromComparer.Instance) { { typeof(Object), typeof(DrawInspectorHandler) } }
                }
            };
        }

        protected override HashSet<Type> GenerateAvailable()
        {
            return new HashSet<Type>(AssignableFromComparer.Instance) { typeof(Object) };
        }
    }
}