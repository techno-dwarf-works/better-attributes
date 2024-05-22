using System;
using Better.Commons.Runtime.Utility;
using UnityEngine;

namespace Better.Commons.Runtime.Extensions
{
    public static class CanvasGroupExtensions
    {
        /// <summary>
        /// Changing canvas visibility and interactivity
        /// </summary>
        /// <param name="self"></param>
        /// <param name="isVisible"></param>
        public static void SetActive(this CanvasGroup self, bool isVisible)
        {
            if (self.IsNullOrDestroyed())
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return;
            }

            self.alpha = isVisible ? 1 : 0;
            self.interactable = isVisible;
            self.blocksRaycasts = isVisible;
        }
    }
}