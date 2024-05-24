using System;
using System.Collections.Generic;
using System.Reflection;
using Better.Attributes.EditorAddons.Drawers.Select.SetupStrategies;
using Better.Attributes.EditorAddons.Drawers.Utility;
using Better.Attributes.EditorAddons.Drawers.WrapperCollections;
using Better.Attributes.EditorAddons.Extensions;
using Better.Attributes.Runtime.Select;
using Better.Commons.EditorAddons.Drawers;
using Better.Commons.EditorAddons.Drawers.Attributes;
using Better.Commons.EditorAddons.Drawers.Base;
using Better.Commons.EditorAddons.Drawers.Caching;
using Better.Commons.EditorAddons.DropDown;
using Better.Commons.EditorAddons.Enums;
using Better.Commons.EditorAddons.Extensions;
using Better.Commons.EditorAddons.Helpers;
using Better.Commons.EditorAddons.Utility;
using Better.Commons.Runtime.Drawers.Attributes;
using Better.Commons.Runtime.Extensions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

#pragma warning disable CS0618
namespace Better.Attributes.EditorAddons.Drawers.Select
{
    [MultiCustomPropertyDrawer(typeof(SelectAttribute))]
    [MultiCustomPropertyDrawer(typeof(DropdownAttribute))]
    public class SelectDrawer : MultiFieldDrawer<BaseSelectWrapper>
    {
        private bool _needUpdate;
        private DisplayName _displayName;
        private DisplayGrouping _displayGrouping;

        protected SelectedItem<object> _selectedItem;
        protected List<object> _selectionObjects;
        protected SetupStrategy _setupStrategy;

        protected SelectHandlers Collection => _handlers as SelectHandlers;

        protected SelectDrawer(FieldInfo fieldInfo, MultiPropertyAttribute attribute) : base(fieldInfo, attribute)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            var attribute = (SelectAttributeBase)_attribute;
            _displayName = attribute.DisplayName;
            _displayGrouping = attribute.DisplayGrouping;
        }

        protected override void PopulateContainer(ElementsContainer container)
        {
            var property = container.Property;
            var attribute = (SelectAttributeBase)_attribute;
            if (_setupStrategy == null || !_setupStrategy.CheckSupported())
            {
                container.AddNotSupportedBox(GetFieldOrElementType(), _attribute.GetType());
                return;
            }

            if (_setupStrategy.IsSkipingFieldDraw() && container.TryGetPropertyField(out var propertyField))
            {
                propertyField.style.SetVisible(false);
            }

            _selectionObjects ??= _setupStrategy.Setup();
            InitializeSetupStrategy(container.Property, attribute);

            var cache = ValidateCachedProperties(property, __SelectUtility.Instance);
            if (!cache.IsValid)
            {
                cache.Value.Wrapper.Setup(property, _fieldInfo, _attribute, _setupStrategy);
            }

            var button = CreateButton(property, OnButtonClick);

            if (container.TryGetByTag(container.Property, out var fieldElement))
            {
                fieldElement.Elements.Add(button);
            }
        }

        private Button CreateButton(SerializedProperty property, EventCallback<ClickEvent, (SerializedProperty, Button)> onButtonClick)
        {
            var content = IconType.GrayDropdown.GetIconGUIContent();

            var currentValue = GetCurrentValue(property);
            content.text = _setupStrategy.GetButtonName(currentValue);
            var button = new Button();
            button.RegisterCallback(onButtonClick, (property, button));
            return button;
        }

        private void OnButtonClick(ClickEvent clickEvent, (SerializedProperty property, Button button) data)
        {
            _selectionObjects = _setupStrategy.Setup();
            var property = data.property;
            var value = GetCurrentValue(property);
            ShowDropDown(property.Copy(), data.button.worldBound, value);
        }

        protected override void Deconstruct()
        {
            base.Deconstruct();
            DropdownWindow.CloseInstance();
            _selectionObjects = null;
            _setupStrategy = null;
        }

        private void ShowDropDown(SerializedProperty serializedProperty, Rect popupPosition, object currentValue)
        {
            var copy = popupPosition;
            copy.y += EditorGUIUtility.singleLineHeight;

            var popup = DropdownWindow.ShowWindow(GUIUtility.GUIToScreenRect(copy), _setupStrategy.GenerateHeader());
            var items = GenerateItemsTree(serializedProperty, currentValue);

            popup.SetItems(items);
        }

        private DropdownCollection GenerateItemsTree(SerializedProperty serializedProperty, object currentValue)
        {
            var items = new DropdownCollection(new DropdownSubTree(new GUIContent("Root")));
            if (_displayGrouping == DisplayGrouping.None)
            {
                foreach (var value in _selectionObjects)
                {
                    var guiContent = _setupStrategy.ResolveName(value, _displayName);
                    if (guiContent == null)
                    {
                        continue;
                    }

                    if (guiContent.image == null && _setupStrategy.ResolveState(currentValue, value))
                    {
                        guiContent.image = IconType.Checkmark.GetIcon();
                    }

                    var item = new DropdownItem(guiContent, OnSelectItem, new SelectedItem<object>(serializedProperty, value));
                    items.AddChild(item);
                }
            }
            else
            {
                foreach (var type in _selectionObjects)
                {
                    var resolveGroupedName = _setupStrategy.ResolveGroupedName(type, _displayGrouping);
                    items.AddItem(resolveGroupedName, _setupStrategy.ResolveState(currentValue, type), OnSelectItem,
                        new SelectedItem<object>(serializedProperty, type));
                }
            }

            return items;
        }

        private void InitializeSetupStrategy(SerializedProperty property, SelectAttributeBase attribute)
        {
            _setupStrategy ??= __SelectUtility.Instance.GetSetupStrategy(_fieldInfo, property.GetLastNonCollectionContainer(), attribute);
        }

        private object GetCurrentValue(SerializedProperty property)
        {
            return Collection.GetCurrentValue(property);
        }

        private void OnSelectItem(object obj)
        {
            if (obj is SelectedItem<object> value && !_setupStrategy.Validate(value.Data))
            {
                return;
            }

            if (obj == null)
            {
                Update(null);
                return;
            }

            var item = (SelectedItem<object>)obj;
            if (_selectedItem != default)
            {
                if (_selectedItem.Equals(item))
                {
                    return;
                }
            }

            Update(item);
        }

        protected override HandlerCollection<BaseSelectWrapper> GenerateCollection()
        {
            return new SelectHandlers();
        }

        protected void Update(SelectedItem<object> selectedItem)
        {
            Collection.Update(selectedItem);
            _selectedItem = selectedItem;
        }
    }
}