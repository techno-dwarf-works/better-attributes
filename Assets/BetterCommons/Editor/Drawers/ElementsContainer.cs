using System;
using System.Collections.Generic;
using System.Linq;
using Better.Commons.Runtime.Utility;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Better.Commons.EditorAddons.Drawers
{
    public class ElementsContainer
    {
        public SerializedProperty Property { get; }
        public SerializedObject Object => Property.serializedObject;
        private SortedSet<FieldVisualElement> _elements;
        private VisualElement _container;
        private IStyle _style;

        public IStyle ContainerStyle => _container.style;
        public event Action<ElementsContainer> OnSerializedObjectChanged;

        public ElementsContainer(SerializedProperty property)
        {
            Property = property;
            _elements = new SortedSet<FieldVisualElement>();
            _container = new VisualElement();
            _container.TrackSerializedObjectValue(Object, Callback);
        }

        public FieldVisualElement CreateElementFrom(VisualElement element)
        {
            var item = new FieldVisualElement(element);
            _elements.Add(item);
            return item;
        }

        public void SetEnabled(bool value)
        {
            _container.SetEnabled(value);
        }

        public FieldVisualElement CreateElement()
        {
            var item = new FieldVisualElement();
            _elements.Add(item);
            return item;
        }

        public void RemoveByTag(object tag)
        {
            _elements.RemoveWhere(x => x.ContainsTag(tag));
        }

        public FieldVisualElement GetByTag(object tag)
        {
            if (TryGetByTag(tag, out var fieldVisualElement))
            {
                return fieldVisualElement;
            }

            throw new KeyNotFoundException($"Element with tag: {tag} not found");
        }

        public IEnumerable<FieldVisualElement> GetByTags(IEnumerable<object> tag)
        {
            return _elements.Where(x => x.ContainsAnyTags(tag));
        }

        public bool TryGetByTag(object tag, out FieldVisualElement element)
        {
            element = _elements.FirstOrDefault(x => x.ContainsTag(tag));
            return element != null;
        }

        public IEnumerable<FieldVisualElement> GetAll()
        {
            return _elements;
        }

        public void ClearElements()
        {
            _elements.Clear();
        }

        public VisualElement Generate()
        {
            foreach (var value in _elements)
            {
                var visualElement = value.Generate();
                _container.Add(visualElement);
            }

            return _container;
        }

        private void Callback(SerializedObject obj)
        {
            OnSerializedObjectChanged?.Invoke(this);
        }
    }
}