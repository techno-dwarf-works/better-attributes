using System.Reflection;
using Better.Internal.Core.Runtime;
using UnityEditor;
using UnityEditorInternal;

namespace Better.Commons.EditorAddons.Utility
{
    public static class ReorderableListUtility
    {
        private static MethodInfo _repaintInspectors = null;

        static ReorderableListUtility()
        {
            var inspWin = typeof(ReorderableList);
            _repaintInspectors = inspWin.GetMethod("InvalidateParentCaches", Defines.MethodFlags);
        }

        //TODO: Need to find better way to refresh ReorderableList
        public static void RepaintAllInspectors(SerializedProperty property)
        {
            if (_repaintInspectors != null) _repaintInspectors.Invoke(null, new object[] { property.propertyPath });
        }
    }
}