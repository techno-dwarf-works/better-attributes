using System;
using Better.Commons.EditorAddons.Drawers.Utility;

namespace Better.Commons.EditorAddons.Drawers.Base
{
    public class CollectionValue<T> where T : SerializedPropertyHandler
    {
        public CollectionValue(T wrapper, Type type)
        {
            Wrapper = wrapper;
            Type = type;
        }

        public T Wrapper { get; }
        public Type Type { get; }
    }
}