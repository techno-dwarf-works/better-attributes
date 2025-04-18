﻿using Better.Attributes.Runtime.Misc;
using Better.Commons.EditorAddons.Drawers;
using Better.Commons.EditorAddons.Drawers.HandlerBinding;
using Better.Commons.EditorAddons.Helpers;

namespace Better.Attributes.EditorAddons.Drawers.Misc
{
    [HandlerBinding(typeof(RenameFieldAttribute))]
    public class RenameFieldHandler : MiscLabelHandler
    {
        protected override void OnUpdateLabel(LabelContainer labelContainer)
        {
            var name = ((RenameFieldAttribute)_attribute).Name;
            labelContainer.Text = name;
        }
    }
}