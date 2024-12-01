using Better.Attributes.Runtime.Manipulation;
using Better.Commons.EditorAddons.Drawers;
using UnityEditor;

namespace Better.Attributes.EditorAddons.Drawers.Manipulation
{
    [HandlerBinding(typeof(DisableInPlayModeAttribute))]
    [HandlerBinding(typeof(EnableInPlayModeAttribute))]
    [HandlerBinding(typeof(ShowInPlayModeAttribute))]
    [HandlerBinding(typeof(HideInPlayModeAttribute))]
    public class InPlayModeHandler : ManipulateHandler
    {
        protected override bool IsConditionSatisfied()
        {
            return EditorApplication.isPlaying;
        }
    }
}