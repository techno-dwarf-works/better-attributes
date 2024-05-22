using System.Reflection;
using Better.Commons.EditorAddons.Drawers.Caching;
using Better.Commons.Runtime.Drawers.Attributes;
using UnityEditor;
using UnityEngine;

namespace Better.Commons.EditorAddons.Drawers.Base
{
    public abstract class FieldDrawer
    {
        protected readonly FieldInfo _fieldInfo;
        protected readonly MultiPropertyAttribute _attribute;
        protected FieldDrawer _nextDrawer;

        protected FieldDrawer(FieldInfo fieldInfo, MultiPropertyAttribute attribute)
        {
            Selection.selectionChanged += OnSelectionChanged;
            _fieldInfo = fieldInfo;
            _attribute = attribute;
        }

        ~FieldDrawer()
        {
            EditorApplication.update += DeconstructOnMainThread;
        }

        public virtual void Initialize(FieldDrawer drawer)
        {
            _nextDrawer = drawer;
        }

        private void DeconstructOnMainThread()
        {
            EditorApplication.update -= DeconstructOnMainThread;
            Selection.selectionChanged -= OnSelectionChanged;
            Deconstruct();
        }

        private void OnSelectionChanged()
        {
            Selection.selectionChanged -= OnSelectionChanged;
            Deconstruct();
        }

        protected abstract void Deconstruct();
        
        internal void PopulateContainerInternal(ElementsContainer elementsContainer)
        {
            if (_nextDrawer != null)
            {
                _nextDrawer.PopulateContainerInternal(elementsContainer);
            }

            PopulateContainer(elementsContainer);
        }

        protected abstract void PopulateContainer(ElementsContainer container);
    }
}