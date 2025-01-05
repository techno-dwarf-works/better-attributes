using Better.Attributes.Runtime.Manipulation;
using Better.Commons.EditorAddons.Drawers;
using Better.Commons.EditorAddons.Drawers.HandlerBinding;

namespace Better.Attributes.EditorAddons.Drawers.Manipulation
{
    [HandlerBinding(typeof(ReadOnlyAttribute))]
    public class ReadOnlyFieldAttributeHandler : ManipulateHandler
    {
        protected override bool IsConditionSatisfied()
        {
            return true;
        }
    }
}