using System;
using UnityEngine;

namespace Better.Commons.EditorAddons.DropDown.Base
{
    public abstract class DropdownBase
    {
        protected DropdownBase(GUIContent content)
        {
            Content = content;
        }

        public GUIContent Content { get; }
        
        internal abstract bool Invoke(DropdownWindow downPopup);

        public bool Equals(string value)
        {
            return Content.text.Equals(value);
        }

        public bool Contains(string searchText,
            StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
#if UNITY_2022_3_OR_NEWER
            return Content.text.Contains(searchText, comparison);
#else
            return Content.text.IndexOf(searchText, 0, comparison) != -1;
#endif
        }
    }
}