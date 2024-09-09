using Better.Attributes.Runtime.Manipulation;
using Better.Commons.EditorAddons.Drawers;
using Better.Commons.EditorAddons.Drawers.Container;
using UnityEditor;

namespace Better.Attributes.EditorAddons.Drawers.Manipulation
{
    [CustomPropertyDrawer(typeof(ManipulateAttribute), true)]
    public class ManipulateDrawer : BasePropertyDrawer<ManipulateHandler, ManipulateAttribute>
    {
        protected override void PopulateContainer(ElementsContainer container)
        {
            var wrapper = GetHandler(container.SerializedProperty);
            wrapper.SetProperty(container.SerializedProperty, Attribute);
            wrapper.PopulateContainer(container);
            container.SerializedObjectChanged += SerializedObjectChanged;
        }

        private void SerializedObjectChanged(ElementsContainer container)
        {
            var wrapper = GetHandler(container.SerializedProperty);
            wrapper.UpdateState(container);
        }
    }
}