using Better.Commons.EditorAddons.Extensions;

namespace Better.Attributes.EditorAddons.Drawers.Select
{
    public class DropdownWrapper : BaseSelectWrapper
    {
        protected override void OnSetup()
        {
            
        }

        public override void Update(object value)
        {
            if (!_property.Verify()) return;
            _property.SetValue(value);
        }

        public override object GetCurrentValue()
        {
            return _property.GetValue();
        }
    }
}