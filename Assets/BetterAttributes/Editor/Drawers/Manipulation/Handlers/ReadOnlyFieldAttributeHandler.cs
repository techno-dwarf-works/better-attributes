namespace Better.Attributes.EditorAddons.Drawers.Manipulation
{
    public class ReadOnlyFieldAttributeHandler : ManipulateHandler
    {
        protected override bool IsConditionSatisfied()
        {
            return true;
        }
    }
}