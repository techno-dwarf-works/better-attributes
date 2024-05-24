using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Better.Attributes.EditorAddons.Drawers.Utility;
using Better.Attributes.Runtime;
using Better.Commons.Runtime.Extensions;

namespace Better.Attributes.EditorAddons.Drawers.Select.SetupStrategies
{
    public class ListCollection : IDataCollection
    {
        private class KeyTupleComparer : IEqualityComparer<ITuple>
        {
            public bool Equals(ITuple x, ITuple y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return Equals(x[0], y[0]);
            }

            public int GetHashCode(ITuple obj)
            {
                return obj.Length;
            }
        }

        private readonly IList _list;
        private readonly bool _showDefault;
        private readonly bool _showUniqueKey;

        public ListCollection(IList list, bool showDefault, bool showUniqueKey)
        {
            _list = list;
            _showDefault = showDefault;
            _showUniqueKey = showUniqueKey;
        }

        public string FindName(object obj)
        {
            if (obj == null)
            {
                return LabelDefines.Null;
            }

            if (_list.Count <= 0) return obj.ToString();
            if (_list[_list.Count - 1] is ITuple)
            {
                foreach (ITuple item in _list)
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
            if (_list.Count <= 0) return new List<object>();
            var last = _list[_list.Count - 1];
            var type = last.GetType();
            object defaultElement;
            IEnumerable<object> objects;
            if (last is ITuple lastTuple)
            {
                var tupleLength = lastTuple.Length - 1;
                defaultElement = lastTuple[tupleLength].GetType().GetDefault();
                if (_showUniqueKey)
                {
                    objects = _list.Cast<ITuple>().Distinct(new KeyTupleComparer()).Select(tuple => tuple[tupleLength]);
                }
                else
                {
                    objects = _list.Cast<ITuple>().Select(tuple => tuple[tupleLength]);
                }
            }
            else
            {
                defaultElement = type.GetDefault();
                objects = _list.Cast<object>();

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