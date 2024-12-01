using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Better.Attributes.EditorAddons.CustomEditors;
using Better.Attributes.EditorAddons.Drawers.EditorButton;
using Better.Attributes.Runtime.Misc;
using Better.Commons.EditorAddons.Drawers;
using Better.Commons.EditorAddons.Drawers.Container;
using Better.Commons.EditorAddons.Extensions;
using Better.Commons.EditorAddons.Helpers;
using Better.Commons.EditorAddons.Utility;
using Better.Commons.Runtime.Extensions;
using Better.Commons.Runtime.Utility;
using Better.Internal.Core.Runtime;
using UnityEditor;
using UnityEngine;

namespace Better.Attributes.EditorAddons.Drawers.Misc
{
    //TODO: add parameters
    
    [HandlerBinding(typeof(DetailedAttribute))]
    public class DetailedHandler : MiscHandler
    {
        private DetailedAttribute _detailedAttribute;
        private EditorButtonDrawer _buttonDrawer;

        protected override void OnSetupContainer()
        {
            _detailedAttribute = (DetailedAttribute)_attribute;

            if (!ValidateNested())
            {
                return;
            }

            var target = GetValueFromContainer(_container);

            _buttonDrawer = new EditorButtonDrawer(_container.SerializedObject);
            _container.CreateElementFrom(_buttonDrawer);

            if (target != null)
            {
                _buttonDrawer.CreateFromTarget(target);
            }

            _container.SerializedPropertyChanged += OnPropertyChanged;
        }

        private bool ValidateNested()
        {
            if (!_detailedAttribute.Nested)
            {
                return true;
            }

            var serializedProperty = _container.SerializedProperty;
            var list = new List<PropertyParent>();
            serializedProperty.CollectPropertyParents(ref list);

            for (var i = list.Count - 1; i >= 0; i--)
            {
                var valueTuple = list[i];
                var container = valueTuple.ParentInstance;
                var fieldName = valueTuple.FieldName;

                if (fieldName.IsNullOrEmpty() || container.GetType().IsEnumerable())
                {
                    continue;
                }

                var field = container.GetType().GetField(fieldName, Defines.FieldsFlags);

                if (field == null || field.GetCustomAttributes(typeof(DetailedAttribute), true).Length <= 0)
                {
                    return false;
                }

                if (container is MonoBehaviour or ScriptableObject)
                {
                    return true;
                }
            }

            return false;
        }

        private object GetValueFromContainer(ElementsContainer elementsContainer)
        {
            var property = elementsContainer.SerializedProperty;
            switch (property.propertyType)
            {
                case SerializedPropertyType.Generic:
                    return property.GetValue();
                case SerializedPropertyType.ManagedReference:
                    return property.managedReferenceValue;
                default:
                    return null;
            }
        }

        private void OnPropertyChanged(ElementsContainer container)
        {
            var target = GetValueFromContainer(_container);

            if (target == null)
            {
                _buttonDrawer.Clear();
                return;
            }

            _buttonDrawer.CreateFromTarget(target);
        }

        public override void Deconstruct()
        {
            _container.SerializedPropertyChanged -= OnPropertyChanged;
        }
    }
}