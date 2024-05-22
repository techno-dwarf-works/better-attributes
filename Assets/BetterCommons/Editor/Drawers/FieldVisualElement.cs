using System;
using System.Collections.Generic;
using Better.Commons.Runtime.Utility;
using UnityEngine.UIElements;

namespace Better.Commons.EditorAddons.Drawers
{
    public class FieldVisualElement : IComparable<FieldVisualElement>
    {
        private readonly VisualElement _root;

        private readonly HashSet<object> _tags;

        public List<VisualElement> Elements { get; private set; }
        public int Order { get; set; }
        public IStyle RootStyle => _root.style;

        public FieldVisualElement()
        {
            Elements = new List<VisualElement>();
            _root = new VisualElement();
            _tags = new HashSet<object>();
        }

        public FieldVisualElement(VisualElement element) : this()
        {
            Elements.Add(element);
        }

        public bool ContainsTag(object value)
        {
            return _tags != null && _tags.Contains(value);
        }

        public bool ContainsAllTags(IEnumerable<object> values)
        {
            if (values == null)
            {
                var message = $"{nameof(values)} cannot be null";
                DebugUtility.LogException(message);
                return false;
            }

            foreach (var value in values)
            {
                if (!ContainsTag(value))
                {
                    return false;
                }
            }

            return true;
        }

        public bool ContainsAnyTags(IEnumerable<object> values)
        {
            if (values == null)
            {
                var message = $"{nameof(values)} cannot be null";
                DebugUtility.LogException(message);
                return false;
            }

            foreach (var value in values)
            {
                if (ContainsTag(value))
                {
                    return true;
                }
            }

            return false;
        }

        public void AddTag(object value)
        {
            if (value == null)
            {
                var message = $"{nameof(value)} cannot be null";
                DebugUtility.LogException(message);
                return;
            }

            if (!ContainsTag(value))
            {
                _tags.Add(value);
            }
        }

        public void AddTags(IEnumerable<object> values)
        {
            if (values == null)
            {
                var message = $"{nameof(values)} cannot be null";
                DebugUtility.LogException(message);
                return;
            }

            foreach (var value in values)
            {
                AddTag(value);
            }
        }

        public void RemoveTag(object value)
        {
            _tags?.Remove(value);
        }

        public void RemoveTags(IEnumerable<object> values)
        {
            if (values == null)
            {
                var message = $"{nameof(values)} cannot be null";
                DebugUtility.LogException(message);
                return;
            }

            foreach (var value in values)
            {
                RemoveTag(value);
            }
        }

        public int CompareTo(FieldVisualElement other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return Order.CompareTo(other.Order);
        }

        public VisualElement Generate()
        {
            foreach (var visualElement in Elements)
            {
                _root.Add(visualElement);
            }

            return _root;
        }
    }
}