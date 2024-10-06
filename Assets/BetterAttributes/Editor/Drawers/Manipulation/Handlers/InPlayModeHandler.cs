using UnityEditor;

namespace Better.Attributes.EditorAddons.Drawers.Manipulation
{
    public class InPlayModeHandler : ManipulateHandler
    {
        protected override bool IsConditionSatisfied()
        {
            return EditorApplication.isPlaying;
        }
    }
}