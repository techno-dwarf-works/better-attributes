namespace Better.Attributes.EditorAddons.Drawers.Manipulation
{
    public class ReadOnlyFieldAttributeWrapper : ManipulateWrapper
    {
        protected override bool IsConditionSatisfied()
        {
            return true;
        }
    }
}