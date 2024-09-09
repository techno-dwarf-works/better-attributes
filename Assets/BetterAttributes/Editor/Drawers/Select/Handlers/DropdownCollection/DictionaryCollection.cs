using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Better.Attributes.Runtime;
using Better.Commons.Runtime.Extensions;
using UnityEngine;

namespace Better.Attributes.EditorAddons.Drawers.Select
{
    public class DictionaryCollection : IDataCollection
    {
        private readonly IDictionary _dictionary;
        private readonly bool _showDefault;
        private readonly bool _showUniqueKey;

        public DictionaryCollection(IDictionary dictionary, bool showDefault, bool showUniqueKey)
        {
            _dictionary = dictionary;
            _showDefault = showDefault;
            _showUniqueKey = showUniqueKey;
        }

        public string FindName(object obj)
        {
            if (obj == null)
            {
                return LabelDefines.Null;
            }

            if (_dictionary.Count <= 0) return obj.ToString();

            foreach (DictionaryEntry en in _dictionary)
            {
                if (en.Value.Equals(obj))
                {
                    return en.Key as string;
                }
            }

            return obj.ToString();
        }

        public List<object> GetValues()
        {
            Type type = null;
            if (_dictionary.Count <= 0) return new List<object>();
            foreach (DictionaryEntry entry in _dictionary)
            {
                type = entry.Value.GetType();
                break;
            }

            if (type == null) return new List<object>();
            var defaultElement = type.GetDefault();
            var objects = _dictionary.Values.Cast<object>().ToList();

            if (_showUniqueKey)
            {
                Debug.LogWarning($"{nameof(_showUniqueKey)} currently not supported by dictionary collection!");
            }

            if (_showDefault)
            {
                if (!objects.Contains(defaultElement, EqualityComparer<object>.Default))
                {
                    objects.Insert(0, defaultElement);
                }
            }

            return objects.ToList();
        }
    }
}