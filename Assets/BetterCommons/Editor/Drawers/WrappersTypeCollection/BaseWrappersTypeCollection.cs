using System;
using System.Collections;
using System.Collections.Generic;

namespace Better.Commons.EditorAddons.Drawers.WrappersTypeCollection
{
    public abstract class BaseWrappersTypeCollection : IEnumerable<Type>
    {
        public abstract bool TryGetValue(Type attributeType, Type fieldType, out Type wrapperType);
        public abstract IEnumerator<Type> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}