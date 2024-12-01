using Better.Attributes.Runtime.Manipulation;
using Better.Commons.EditorAddons.Drawers;
using UnityEditor;

namespace Better.Attributes.EditorAddons.Drawers.Manipulation
{
    [HandlerBinding(typeof(DisableInEditorModeAttribute))]
    [HandlerBinding(typeof(EnableInEditorModeAttribute))]
    [HandlerBinding(typeof(ShowInEditorModeAttribute))]
    [HandlerBinding(typeof(HideInEditorModeAttribute))]
    public class InEditorModeHandler : ManipulateHandler
    {
        protected override bool IsConditionSatisfied()
        {
            return !EditorApplication.isPlaying;
        }
    }
}