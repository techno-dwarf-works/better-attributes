using Better.Commons.EditorAddons.Drawers;
using Better.Commons.EditorAddons.Drawers.Container;
using Better.Commons.EditorAddons.Extensions;
using Better.Commons.EditorAddons.Helpers;
using Better.Commons.Runtime.Extensions;
using UnityEngine.UIElements;

namespace Better.Attributes.EditorAddons.Drawers.Misc
{
    public class HideLabelHandler : MiscLabelHandler
    {
        protected override void OnUpdateLabel(LabelContainer labelContainer)
        {
            labelContainer.Style.SetVisible(false);
            _container.SerializedPropertyChanged += OnSerializedPropertyChanged;
            OnSerializedPropertyChanged(_container);
        }

        private void OnSerializedPropertyChanged(ElementsContainer container)
        {
            var property = _container.SerializedProperty;
            if (property.hasChildren)
            {
                property.isExpanded = true;
                _container.RootElement.OnElementAppear<Foldout>(OnFoldoutAppear);
            }
        }

        private void OnFoldoutAppear(Foldout foldout)
        {
            var toggle = foldout.Q<Toggle>();
            toggle.style.SetVisible(false);
            var marginLeft = new StyleLength(new Length(0));

            foldout.contentContainer.style.MarginLeft(marginLeft);
            _container.RootElement.style.MarginLeft(marginLeft);
        }
    }
}