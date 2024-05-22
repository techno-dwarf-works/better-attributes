using System;
using System.Collections.Generic;

namespace Better.Commons.EditorAddons.Drawers.WrappersTypeCollection
{
    public class WrappersFieldTypeCollection : BaseWrappersTypeCollection
    {
        protected Dictionary<Type, Type> _dictionary;
        
        public WrappersFieldTypeCollection()
        {
            _dictionary = new Dictionary<Type, Type>();
        }

        public WrappersFieldTypeCollection(IEqualityComparer<Type> equalityComparer)
        {
            _dictionary = new Dictionary<Type, Type>(equalityComparer);
        }

        public override bool TryGetValue(Type attributeType, Type fieldType, out Type wrapperType)
        {
            if (_dictionary.TryGetValue(fieldType, out var type))
            {
                wrapperType = type;
                return true;
            }

            wrapperType = null;
            return false;
        }

        public override IEnumerator<Type> GetEnumerator()
        {
            return ((IEnumerable<Type>)_dictionary.Keys).GetEnumerator();
        }

        public void Add(Type fieldType, Type wrapperType)
        {
            _dictionary.Add(fieldType, wrapperType);
        }
    }
}