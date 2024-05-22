using System;
using System.Collections.Generic;
using System.Linq;
using Better.Commons.Runtime.Utility;
using UnityEngine;

namespace Better.Commons.Runtime.Extensions
{
    public static class RectTransformExtensions
    {
        /// <summary>
        /// Counts the bounding box corners of the given RectTransform that are visible from the given Camera in screen space.
        /// </summary>
        /// <returns>The amount of bounding box corners that are visible from the Camera.</returns>
        /// <param name="self">Rect transform.</param>
        /// <param name="camera">Camera.</param>
        public static IEnumerable<Vector3> CornersVisible(this RectTransform self, Camera camera)
        {
            var corners = self.GetScreenCorners(camera);
            var screenBounds = ScreenUtility.GetScreenBounds();
            return corners.Where(corner => screenBounds.Contains(corner));
        }

        /// <summary>
        /// Counts the bounding box corners of the given RectTransform that are visible from the given Camera in screen space.
        /// </summary>
        /// <returns>The amount of bounding box corners that are visible from the Camera.</returns>
        /// <param name="self">Rect transform.</param>
        /// <param name="camera">Camera.</param>
        public static int CountCornersVisible(this RectTransform self, Camera camera)
        {
            return self.CornersVisible(camera).Count();
        }

        /// <summary>
        /// Counts the bounding box corners of the given RectTransform that are invisible from the given Camera in screen space.
        /// </summary>
        /// <returns>The amount of bounding box corners that are visible from the Camera.</returns>
        /// <param name="self">Rect transform.</param>
        /// <param name="camera">Camera.</param>
        public static IEnumerable<Vector3> CornersInvisible(this RectTransform self, Camera camera)
        {
            var corners = self.GetScreenCorners(camera);
            var screenBounds = ScreenUtility.GetScreenBounds();
            return corners.Where(corner => !screenBounds.Contains(corner));
        }

        /// <summary>
        /// Counts the bounding box corners of the given RectTransform that are visible from the given Camera in screen space.
        /// </summary>
        /// <returns>The amount of bounding box corners that are visible from the Camera.</returns>
        /// <param name="self">Rect transform.</param>
        /// <param name="camera">Camera.</param>
        public static int CountCornersNotVisible(this RectTransform self, Camera camera)
        {
            return self.CornersInvisible(camera).Count();
        }

        public static IEnumerable<Vector3> GetScreenCorners(this RectTransform self, Camera camera)
        {
            if (self.IsNullOrDestroyed())
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return Enumerable.Empty<Vector3>();
            }

            if (camera.IsNullOrDestroyed())
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(camera));
                return Enumerable.Empty<Vector3>();
            }

            var objectCorners = new Vector3[4];
            self.GetWorldCorners(objectCorners);
            return objectCorners.Select(camera.WorldToScreenPoint);
        }

        // TODO: Make better readable
        public static void KeepFullyOnScreen(this RectTransform self, RectTransform container)
        {
            if (self.IsNullOrDestroyed())
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return;
            }

            if (container.IsNullOrDestroyed())
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(container));
                return;
            }

            var cornersCache = new Vector3[4];
            container.GetWorldCorners(cornersCache);

            // BL = Bottom Left, TR = Top Right (corners)
            Vector3 containerBL = cornersCache[0], containerTR = cornersCache[2];
            var containerSize = containerTR - containerBL; // NEW
            self.GetWorldCorners(cornersCache);
            Vector3 movableBL = cornersCache[0], movableTR = cornersCache[2];
            var movableSize = movableTR - movableBL; // NEW
            var position = self.position;
            Vector3 deltaBL = position - movableBL, deltaTR = movableTR - position;

            position.x = movableSize.x < containerSize.x // NEW
                ? Mathf.Clamp(position.x, containerBL.x + deltaBL.x, containerTR.x - deltaTR.x)
                : Mathf.Clamp(position.x, containerTR.x - deltaTR.x, containerBL.x + deltaBL.x); // NEW

            position.y = movableSize.y < containerSize.y // NEW
                ? Mathf.Clamp(position.y, containerBL.y + deltaBL.y, containerTR.y - deltaTR.y)
                : Mathf.Clamp(position.y, containerTR.y - deltaTR.y, containerBL.y + deltaBL.y); // NEW
            self.position = position;
        }

        /// <summary>
        /// Determines if this RectTransform is fully visible from the specified camera.
        /// Works by checking if each bounding box corner of this RectTransform is inside the cameras screen space view frustrum.
        /// </summary>
        /// <returns><c>true</c> if is fully visible from the specified camera; otherwise, <c>false</c>.</returns>
        /// <param name="self">Rect transform.</param>
        /// <param name="camera">Camera.</param>
        public static bool IsFullyVisibleFrom(this RectTransform self, Camera camera)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return false;
            }

            if (!self.gameObject.activeInHierarchy)
            {
                return false;
            }

            return CountCornersVisible(self, camera) == 4; // True if all 4 corners are visible
        }

        /// <summary>
        /// Determines if this RectTransform is at least partially visible from the specified camera.
        /// Works by checking if any bounding box corner of this RectTransform is inside the cameras screen space view frustrum.
        /// </summary>
        /// <returns><c>true</c> if is at least partially visible from the specified camera; otherwise, <c>false</c>.</returns>
        /// <param name="self">Rect transform.</param>
        /// <param name="camera">Camera.</param>
        public static bool IsVisibleFrom(this RectTransform self, Camera camera)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return false;
            }

            if (!self.gameObject.activeInHierarchy)
            {
                return false;
            }

            return CountCornersVisible(self, camera) > 0; // True if any corners are visible
        }

        public static bool IsFullyStretched(this RectTransform self)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return false;
            }

            return self.localScale == Vector3.one
                   && self.anchorMin == Vector2.zero
                   && self.anchorMax == Vector2.one
                   && self.sizeDelta == Vector2.zero;
        }

        public static void FullyStretch(this RectTransform self)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return;
            }

            self.LocalReset();
            self.anchorMin = Vector2.zero;
            self.anchorMax = Vector2.one;
            self.sizeDelta = Vector2.zero;
        }
    }
}