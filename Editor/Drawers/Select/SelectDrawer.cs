using System.Collections.Generic;
using Better.Attributes.Runtime.Select;
using Better.Commons.EditorAddons.Comparers;
using Better.Commons.EditorAddons.Drawers;
using Better.Commons.EditorAddons.Drawers.BehavioredElements;
using Better.Commons.EditorAddons.Drawers.Container;
using Better.Commons.EditorAddons.DropDown;
using Better.Commons.EditorAddons.Extensions;
using Better.Commons.EditorAddons.Utility;
using Better.Commons.Runtime.Extensions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Better.Attributes.EditorAddons.Drawers.Select
{
    [CustomPropertyDrawer(typeof(SelectAttribute), true)]
    [CustomPropertyDrawer(typeof(DropdownAttribute), true)]
    public class SelectDrawer : PropertyDrawer<BaseSelectHandler, BaseSelectAttribute>
    {
        //TODO: Consider to use Locators
        private Dictionary<SerializedProperty, BehavioredElement<Button>> _behavioredElements;

        public SelectDrawer()
        {
            _behavioredElements = new Dictionary<SerializedProperty, BehavioredElement<Button>>(SerializedPropertyComparer.Instance);
        }

        protected override void PopulateContainer(ElementsContainer container)
        {
            var property = container.SerializedProperty;

            var handler = GetHandler(property);
            handler.Setup(container, FieldInfo, Attribute);
            if (!handler.CheckSupported())
            {
                container.AddNotSupportedBox(GetFieldOrElementType(), Attribute.GetType());
                return;
            }

            container.RootElement.style.MinHeight(StyleDefinition.SingleLineHeight);

            var element = container.CoreElement;
            if (handler.IsSkippingFieldDraw())
            {
                element.style.SetVisible(false);
            }

            var behavioredElement = GetOrCreateBehavioredElement(container);
            behavioredElement.SubElement.text = handler.GetButtonText();

            container.RootElement.RegisterCallback<ReferenceTypeChangeEvent, ElementsContainer>(OnReferenceTypeChange, container);
            container.SerializedPropertyChanged += UpdateElement;

            handler.OnPopulateContainer();

            if(container.TryGetByTag(ElementsContainer.Tag, out var containerPrewarmElement))
            {
                containerPrewarmElement.OnElementAppear<Label>(behavioredElement.Attach).Until(null).Every(100);
            }
        }

        private void OnReferenceTypeChange(ReferenceTypeChangeEvent changeEvent, ElementsContainer container)
        {
            UpdateElement(container);
        }

        private void UpdateElement(ElementsContainer container)
        {
            var handler = GetHandler(container.SerializedProperty);
            if (!handler.IsValid)
            {
                handler.Setup(container, FieldInfo, Attribute);
            }

            var buttonText = handler.GetButtonText();

            var label = container.RootElement.Q<Label>();

            var element = GetOrCreateBehavioredElement(container);
            element.SubElement.text = buttonText;
            element.Attach(label);
        }

        private BehavioredElement<Button> GetOrCreateBehavioredElement(ElementsContainer container)
        {
            var key = container.SerializedProperty;
            if (_behavioredElements.TryGetValue(key, out var element))
            {
                return element;
            }

            element = CreateBehavioredElement(container);
            _behavioredElements.Add(key, element);
            return element;
        }

        private BehavioredElement<Button> CreateBehavioredElement(ElementsContainer container)
        {
            var element = new BehavioredElement<Button>(new SelectElementBehaviour());
            element.RegisterCallback<ClickEvent, ElementsContainer, BehavioredElement<Button>>(OnButtonClick, container, element);
            return element;
        }

        private void OnButtonClick(ClickEvent clickEvent, (ElementsContainer container, BehavioredElement<Button> decorator) data)
        {
            var dataButton = data.decorator;
            ShowDropDown(data.container, dataButton.worldBound);
        }

        protected override void ContainerReleased(ElementsContainer container)
        {
            base.ContainerReleased(container);
            DropdownWindow.CloseInstance();
        }

        private void ShowDropDown(ElementsContainer container, Rect popupPosition)
        {
            var copy = popupPosition;
            copy.y += StyleDefinition.SingleLineHeight.value.value;
            var handler = GetHandler(container.SerializedProperty);
            if (!handler.IsValid)
            {
                handler.Setup(container, FieldInfo, Attribute);
            }

            var popup = DropdownWindow.ShowWindow(GUIUtility.GUIToScreenRect(copy), handler.GenerateHeader());
            var items = handler.GenerateItemsTree();

            popup.SetItems(items);
        }
    }
}