using Better.Attributes.Runtime.Misc;
using Better.Commons.EditorAddons.Drawers;
using Better.Commons.EditorAddons.Drawers.Container;
using UnityEditor;

namespace Better.Attributes.EditorAddons.Drawers.Misc
{
    [CustomPropertyDrawer(typeof(MiscAttribute), true)]
    public class MiscDrawer : BasePropertyDrawer<MiscHandler, MiscAttribute>
    {
        protected override void PopulateContainer(ElementsContainer container)
        {
            var property = container.SerializedProperty;
            var handler = GetHandler(property);
            handler.SetupContainer(container, FieldInfo, Attribute);
        }
    }
}