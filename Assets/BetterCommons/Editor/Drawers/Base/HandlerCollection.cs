using System.Collections.Generic;
using Better.Commons.EditorAddons.Comparers;
using Better.Commons.EditorAddons.Drawers.Utility;
using UnityEditor;

namespace Better.Commons.EditorAddons.Drawers.Base
{
    public class HandlerCollection<T> : Dictionary<SerializedProperty, CollectionValue<T>>
        where T : SerializedPropertyHandler
    {
        public HandlerCollection() : base(SerializedPropertyComparer.Instance)
        {
        }

        /// <summary>
        /// Deconstruct method for stored wrappers
        /// </summary>
        public void Deconstruct()
        {
            foreach (var value in Values)
            {
                value.Wrapper.Deconstruct();
            }
        }
    }
}