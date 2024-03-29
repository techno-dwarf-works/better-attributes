﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Better.Attributes.EditorAddons.Drawers.Select.SetupStrategies;
using Better.Attributes.EditorAddons.Drawers.Select.Wrappers;
using Better.Attributes.EditorAddons.Drawers.Utilities;
using Better.Attributes.EditorAddons.Drawers.WrapperCollections;
using Better.Attributes.EditorAddons.Extensions;
using Better.Attributes.Runtime.Select;
using Better.EditorTools.EditorAddons.Drawers.Base;
using Better.EditorTools.EditorAddons.Helpers;
using Better.EditorTools.EditorAddons.Helpers.DropDown;
using Better.EditorTools.Runtime.Attributes;
using Better.Extensions.EditorAddons;
using UnityEditor;
using UnityEngine;

namespace Better.Attributes.EditorAddons.Drawers.Select
{
    public abstract class SelectDrawerBase<TAttribute> : MultiFieldDrawer<BaseSelectWrapper> where TAttribute : SelectAttributeBase
    {
        private bool _needUpdate;
        private DisplayName _displayName;
        private DisplayGrouping _displayGrouping;

        protected SelectedItem<object> _selectedItem;
        protected List<object> _selectionObjects;
        protected SetupStrategy _setupStrategy;

        protected SelectWrappers Collection => _wrappers as SelectWrappers;

        protected SelectDrawerBase(FieldInfo fieldInfo, MultiPropertyAttribute attribute) : base(fieldInfo, attribute)
        {
        }

        public override void Initialize(FieldDrawer drawer)
        {
            base.Initialize(drawer);
            var attribute = (TAttribute)_attribute;
            _displayName = attribute.DisplayName;
            _displayGrouping = attribute.DisplayGrouping;
        }

        protected override bool PreDraw(ref Rect position, SerializedProperty property, GUIContent label)
        {
            try
            {
                var attribute = (TAttribute)_attribute;
                _setupStrategy ??= SelectUtility.Instance.GetSetupStrategy(_fieldInfo, property.GetLastNonCollectionContainer(), attribute);
                if (_setupStrategy == null || !_setupStrategy.CheckSupported())
                {
                    EditorGUI.BeginChangeCheck();
                    DrawField(position, property, label);
                    DrawersHelper.NotSupportedAttribute(position, property, label, GetFieldOrElementType(), _attribute.GetType());
                    return false;
                }

                _selectionObjects ??= _setupStrategy.Setup();

                PreDrawExtended(position, property, label);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            return true;
        }

        private void PreDrawExtended(Rect position, SerializedProperty property, GUIContent label)
        {
            var cache = ValidateCachedProperties(property, SelectUtility.Instance);
            if (!cache.IsValid)
            {
                cache.Value.Wrapper.SetProperty(property, _fieldInfo);
            }

            var popupPosition = GetPopupPosition(position, label);

            var referenceValue = GetCurrentValue(property);
            if (DrawButton(popupPosition, referenceValue))
            {
                _selectionObjects = _setupStrategy.Setup();
                ShowDropDown(property.Copy(), popupPosition, referenceValue);
            }

            if (_needUpdate)
            {
                Collection.Update(_selectedItem);
                _needUpdate = false;
                _selectedItem = null;
            }
        }

        private Rect GetPopupPosition(Rect currentPosition, GUIContent label)
        {
            var popupPosition = new Rect(currentPosition);
            var width = label.GetMaxWidth();
            popupPosition.width -= width;
            popupPosition.x += width;
            popupPosition.height = EditorGUIUtility.singleLineHeight;
            return popupPosition;
        }

        protected override void Deconstruct()
        {
            DropdownWindow.CloseInstance();
            _wrappers?.Deconstruct();
            _selectionObjects = null;
            _setupStrategy = null;
        }

        private bool DrawButton(Rect buttonPosition, object currentValue)
        {
            var content = DrawersHelper.GetIconGUIContent(IconType.GrayDropdown);

            content.text = _setupStrategy.GetButtonName(currentValue);
            return GUI.Button(buttonPosition, content, Styles.Button);
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
                        guiContent.image = DrawersHelper.GetIcon(IconType.Checkmark);
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

        protected override HeightCacheValue GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var cache = ValidateCachedProperties(property, SelectUtility.Instance);
            if (!cache.IsValid)
            {
                if (cache.Value == null) return HeightCacheValue.GetAdditive(0f);
                var selectWrapper = cache.Value.Wrapper;
                selectWrapper.SetProperty(property, _fieldInfo);
                return selectWrapper.GetHeight();
            }

            var valueWrapper = cache.Value.Wrapper;
            if (!valueWrapper.Verify())
            {
                valueWrapper.SetProperty(property, _fieldInfo);
            }

            return valueWrapper.GetHeight();
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
                _selectedItem = null;
                SetNeedUpdate();
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

            _selectedItem = item;
            SetNeedUpdate();
        }

        protected override WrapperCollection<BaseSelectWrapper> GenerateCollection()
        {
            return new SelectWrappers();
        }

        protected void SetNeedUpdate()
        {
            _needUpdate = true;
        }

        protected override Rect PreparePropertyRect(Rect original)
        {
            return original;
        }

        protected override void PostDraw(Rect position, SerializedProperty property, GUIContent label)
        {
        }
    }
}