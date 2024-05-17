using Better.Attributes.EditorAddons.Drawers.Select;
using Better.Attributes.EditorAddons.Drawers.Select.Wrappers;
using Better.Commons.EditorAddons.Drawers.Base;
using UnityEditor;

namespace Better.Attributes.EditorAddons.Drawers.WrapperCollections
{
    public class SelectHandlers : HandlerCollection<BaseSelectWrapper>
    {
        public void Update(SelectedItem<object> selectedItem)
        {
            if (TryGetValue(selectedItem.Property, out var value))
            {
                value.Wrapper.Update(selectedItem.Data);
            }
        }

        public object GetCurrentValue(SerializedProperty property)
        {
            if (TryGetValue(property, out var value))
            {
                return value.Wrapper.GetCurrentValue();
            }

            return null;
        }
    }
}