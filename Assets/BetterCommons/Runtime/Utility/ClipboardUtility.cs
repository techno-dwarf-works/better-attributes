using UnityEngine;

namespace Better.Commons.Runtime.Utility
{
    public static class ClipboardUtility
    {
        /// <summary>
        /// Puts the string into the Clipboard.
        /// </summary>
        public static void CopyToClipboard(string value)
        {
            GUIUtility.systemCopyBuffer = value;
        }
    }
}