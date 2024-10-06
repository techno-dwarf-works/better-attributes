using System;
using System.Collections.Generic;
using System.Reflection;
using Better.Attributes.Runtime;
using Better.Attributes.Runtime.Select;
using Better.Attributes.Runtime.Utilities;
using Better.Commons.EditorAddons.Drawers;
using Better.Commons.EditorAddons.Drawers.Container;
using Better.Commons.EditorAddons.Drawers.Handlers;
using Better.Commons.EditorAddons.DropDown;
using Better.Commons.EditorAddons.Enums;
using Better.Commons.EditorAddons.Extensions;
using Better.Commons.EditorAddons.Utility;
using Better.Commons.Runtime.Extensions;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Better.Attributes.EditorAddons.Drawers.Select
{
    public abstract class BaseSelectHandler : SerializedPropertyHandler
    {
        protected object CurrentValue { get; set; }
        protected FieldInfo FieldInfo { get; private set; }
        protected BaseSelectAttribute Attribute { get; private set; }
        protected ElementsContainer Container { get; private set; }
        protected object PropertyContainer { get; private set; }

        public bool IsValid => Container != null && FieldInfo != null && Attribute != null;

        public void Setup(ElementsContainer container, FieldInfo fieldInfo, BaseSelectAttribute attribute)
        {
            Container = container;
            FieldInfo = fieldInfo;
            Attribute = attribute;
            PropertyContainer = Container.SerializedProperty.GetLastNonCollectionParent();
            CurrentValue = GetCurrentValue();
            OnSetup();
        }

        protected abstract void OnSetup();

        public abstract object GetCurrentValue();

        protected abstract List<object> GetObjects();

        protected abstract GUIContent GetResolvedName(object value, DisplayName displayName);
        protected abstract IEnumerable<GUIContent> GetResolvedGroupedName(object value, DisplayGrouping grouping);
        public abstract string GetButtonText();
        protected abstract bool ResolveState(object iteratedValue);
        public abstract bool ValidateSelected(object item);
        public abstract bool CheckSupported();
        public abstract GUIContent GenerateHeader();
        public abstract bool IsSkippingFieldDraw();

        public DropdownCollection GenerateItemsTree()
        {
            var items = new DropdownCollection(new DropdownSubTree(new GUIContent(LabelDefines.Root)));
            var displayGrouping = Attribute.DisplayGrouping;
            var displayName = Attribute.DisplayName;
            var selectionObjects = GetObjects();
            if (displayGrouping == DisplayGrouping.None)
            {
                foreach (var value in selectionObjects)
                {
                    var guiContent = GetResolvedName(value, displayName);
                    if (guiContent == null)
                    {
                        continue;
                    }

                    if (guiContent.image == null && ResolveState(value))
                    {
                        guiContent.image = IconType.Checkmark.GetIcon();
                    }

                    var item = new DropdownItem(guiContent, OnSelectItem, value);
                    items.AddChild(item);
                }
            }
            else
            {
                foreach (var type in selectionObjects)
                {
                    var resolveGroupedName = GetResolvedGroupedName(type, displayGrouping);
                    items.AddItem(resolveGroupedName, ResolveState(type), OnSelectItem, type);
                }
            }

            return items;
        }

        private void OnSelectItem(object obj)
        {
            if (!ValidateSelected(obj))
            {
                return;
            }

            if (obj == null)
            {
                Update(null);
                return;
            }

            if (CurrentValue != default)
            {
                if (CurrentValue.Equals(obj))
                {
                    return;
                }
            }

            Update(obj);
        }

        public virtual void Update(object value)
        {
            CurrentValue = value;

            var property = Container.SerializedProperty;
            property.serializedObject.ApplyModifiedProperties();
        }

        public virtual Type GetFieldOrElementType()
        {
            var t = Attribute.GetFieldType();
            if (t != null)
            {
                return t;
            }

            var fieldType = FieldInfo.FieldType;
            if (fieldType.IsArrayOrList())
                return fieldType.GetCollectionElementType();
            return fieldType;
        }

        public override void Deconstruct()
        {
        }

        public virtual void OnPopulateContainer()
        {
            if (IsSkippingFieldDraw())
            {
                var label = VisualElementUtility.CreateLabelFor(Container.SerializedProperty);
                var element = Container.CreateElementFrom(label);
                element.SendToBack();
                label.style.Width(StyleDefinition.LabelWidthStyle);
                label.SendToBack();
                label.AddToClassList(PropertyField.labelUssClassName);
                label.AddToClassList("field-decoration");
                Container.LabelContainer.Setup(label);
            }
        }
    }
}