using System;
using System.Collections.Generic;

namespace Better.Commons.EditorAddons.Drawers.WrappersTypeCollection
{
    public class WrappersTypeCollection : BaseWrappersTypeCollection
    {
        protected Dictionary<Type, Dictionary<Type, Type>> _dictionary;
        
        public WrappersTypeCollection()
        {
            _dictionary = new Dictionary<Type, Dictionary<Type, Type>>();
        }

        public WrappersTypeCollection(IEqualityComparer<Type> equalityComparer)
        {
            _dictionary = new Dictionary<Type, Dictionary<Type, Type>>(equalityComparer);
        }

        public override bool TryGetValue(Type attributeType, Type fieldType, out Type wrapperType)
        {
            if (_dictionary.TryGetValue(attributeType, out var dictionary))
            {
                if (dictionary.TryGetValue(fieldType, out var type))
                {
                    wrapperType = type;
                    return true;
                }
            }

            wrapperType = null;
            return false;
        }

        public override IEnumerator<Type> GetEnumerator()
        {
            return ((IEnumerable<Type>)_dictionary.Keys).GetEnumerator();
        }

        public void Add(Type attributeType, Dictionary<Type, Type> fieldToWrapper)
        {
            _dictionary.Add(attributeType, fieldToWrapper);
        }
    }
}