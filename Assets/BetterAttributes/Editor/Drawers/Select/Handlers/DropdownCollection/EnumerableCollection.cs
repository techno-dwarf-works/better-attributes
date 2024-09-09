using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Better.Attributes.Runtime;
using Better.Commons.Runtime.Extensions;

namespace Better.Attributes.EditorAddons.Drawers.Select
{
    public class EnumerableCollection : IDataCollection
    {
        private readonly IEnumerable<object> _enumerable;
        private readonly bool _showDefault;
        private readonly bool _showUniqueKey;
        private readonly KeyTupleComparer _keyTupleComparer;

        public EnumerableCollection(IEnumerable enumerable, bool showDefault, bool showUniqueKey)
        {
            _enumerable = enumerable.Cast<object>();
            _showDefault = showDefault;
            _showUniqueKey = showUniqueKey;
            _keyTupleComparer = new KeyTupleComparer();
        }

        public string FindName(object obj)
        {
            if (obj == null)
            {
                return LabelDefines.Null;
            }

            if (!_enumerable.Any()) return obj.ToString();
            if (_enumerable.Last() is ITuple)
            {
                foreach (ITuple item in _enumerable)
                {
                    if (item != null && item[1].Equals(obj))
                    {
                        return item[0].ToString();
                    }
                }
            }

            return obj.ToString();
        }

        public List<object> GetValues()
        {
            if (!_enumerable.Any()) return new List<object>();
            var last = _enumerable.Last();
            var type = last.GetType();
            object defaultElement;
            IEnumerable<object> objects;
            if (last is ITuple lastTuple)
            {
                var tupleLength = lastTuple.Length - 1;
                defaultElement = lastTuple[tupleLength].GetType().GetDefault();
                if (_showUniqueKey)
                {
                    objects = _enumerable.Cast<ITuple>().Distinct(_keyTupleComparer).Select(tuple => tuple[tupleLength]);
                }
                else
                {
                    objects = _enumerable.Cast<ITuple>().Select(tuple => tuple[tupleLength]);
                }
            }
            else
            {
                defaultElement = type.GetDefault();
                objects = _enumerable;

                if (_showUniqueKey)
                {
                    objects = objects.Distinct(EqualityComparer<object>.Default);
                }
            }

            var list = objects.ToList();
            if (_showDefault)
            {
                if (!list.Contains(defaultElement, EqualityComparer<object>.Default))
                {
                    list.Insert(0, defaultElement);
                }
            }

            return list.ToList();
        }
    }
}