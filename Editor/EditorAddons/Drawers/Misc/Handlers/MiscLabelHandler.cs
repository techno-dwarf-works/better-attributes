using Better.Commons.EditorAddons.Drawers;
using Better.Commons.EditorAddons.Helpers;

namespace Better.Attributes.EditorAddons.Drawers.Misc
{
    public abstract class MiscLabelHandler : MiscHandler
    {
        protected override void OnSetupContainer()
        {
            OnUpdateLabel(_container.LabelContainer);
        }

        protected abstract void OnUpdateLabel(LabelContainer labelContainer);
    }
}