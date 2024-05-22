using UnityEngine;

namespace Better.Commons.Runtime.Utility
{
    public static class CursorUtility
    {
        /// <summary>
        /// Changing state of mouse cursor
        /// </summary>
        /// <param name="state"></param>
        public static void SetCursorActive(bool state)
        {
            Cursor.lockState = state ? CursorLockMode.Confined : CursorLockMode.Locked;
            Cursor.visible = state;
        }
    }
}