using UnityEditor;

namespace Better.Attributes.EditorAddons.Drawers.Manipulation
{
    public class InEditorModeHandler : ManipulateHandler
    {
        protected override bool IsConditionSatisfied()
        {
            return !EditorApplication.isPlaying;
        }
    }
}