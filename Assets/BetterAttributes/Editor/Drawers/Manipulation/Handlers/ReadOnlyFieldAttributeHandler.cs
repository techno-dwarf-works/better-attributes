using Better.Attributes.Runtime.Manipulation;
using Better.Commons.EditorAddons.Drawers;

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