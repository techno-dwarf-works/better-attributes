using System;
using System.Linq;
using System.Text;
using Better.Commons.Runtime.Extensions;
using UnityEngine;
using UnityEngine.LowLevel;

namespace Better.Commons.Runtime.Utility
{
    public static class PlayerLoopUtility
    {
        #region Subscribing

        public static void SubscribeToLoop(Type loopType, PlayerLoopSystem.UpdateFunction updateFunction)
        {
            if (updateFunction == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(updateFunction));
                return;
            }

            var currentSystem = PlayerLoop.GetCurrentPlayerLoop();
            ref var loopSystem = ref currentSystem.GetSubSystem(loopType);
            if (loopSystem.type != loopType)
            {
                var message = $"Not found {nameof(loopType)}({loopType.Name})";
                Debug.LogWarning(message);
                return;
            }

            loopSystem.updateDelegate += updateFunction;
            PlayerLoop.SetPlayerLoop(currentSystem);
        }

        public static void SubscribeToLoop<TLoop>(PlayerLoopSystem.UpdateFunction updateFunction)
        {
            var loopType = typeof(TLoop);
            SubscribeToLoop(loopType, updateFunction);
        }

        public static void UnsubscribeFromLoop(Type loopType, PlayerLoopSystem.UpdateFunction updateFunction)
        {
            if (updateFunction == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(updateFunction));
                return;
            }

            var currentSystem = PlayerLoop.GetCurrentPlayerLoop();
            var hasAnyChanges = currentSystem.UnsubscribeRecursive(loopType, updateFunction);
            if (!hasAnyChanges)
            {
                var message = $"Not found {nameof(loopType)}({loopType.Name}) for unsubscribing";
                Debug.LogWarning(message);
                return;
            }

            PlayerLoop.SetPlayerLoop(currentSystem);
        }

        public static void UnsubscribeFromLoop<TLoop>(PlayerLoopSystem.UpdateFunction updateFunction)
        {
            var loopType = typeof(TLoop);
            UnsubscribeFromLoop(loopType, updateFunction);
        }

        public static void Unsubscribe(PlayerLoopSystem.UpdateFunction updateFunction)
        {
            if (updateFunction == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(updateFunction));
                return;
            }

            var currentSystem = PlayerLoop.GetCurrentPlayerLoop();
            var hasAnyChanges = currentSystem.UnsubscribeRecursive(updateFunction);
            if (!hasAnyChanges)
            {
                var message = "Not found any loops for unsubscribing";
                Debug.LogWarning(message);
                return;
            }

            PlayerLoop.SetPlayerLoop(currentSystem);
        }

        #endregion

        #region Add

        public static void AddSubLoop(Type sourceLoopType, Type destinationLoopType, PlayerLoopSystem.UpdateFunction updateFunction)
        {
            if (updateFunction == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(updateFunction));
                return;
            }

            var currentSystem = PlayerLoop.GetCurrentPlayerLoop();
            ref var sourceSystem = ref currentSystem.GetSubSystemRecursive(sourceLoopType);
            if (sourceSystem.type != sourceLoopType)
            {
                var message = $"Not found {nameof(sourceLoopType)}({sourceLoopType.Name})";
                Debug.LogWarning(message);
                return;
            }

            var destinationSystem = new PlayerLoopSystem
            {
                type = destinationLoopType,
                updateDelegate = updateFunction
            };

            if (sourceSystem.subSystemList == null)
            {
                sourceSystem.subSystemList = new[] { destinationSystem };
            }
            else if (sourceSystem.HasSubSystemOf(destinationLoopType))
            {
                var message = $"{nameof(sourceLoopType)}({sourceLoopType.Name}) already contains {nameof(destinationLoopType)}{destinationLoopType}";
                Debug.LogWarning(message);
                return;
            }
            else
            {
                var subSystems = sourceSystem.subSystemList.ToList();
                subSystems.Add(destinationSystem);
                sourceSystem.subSystemList = subSystems.ToArray();
            }

            PlayerLoop.SetPlayerLoop(currentSystem);
        }

        public static void AddSubLoop<TLoopSource, TLoopDestination>(PlayerLoopSystem.UpdateFunction updateFunction)
        {
            if (updateFunction == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(updateFunction));
                return;
            }

            var sourceType = typeof(TLoopSource);
            var destinationType = typeof(TLoopDestination);
            AddSubLoop(sourceType, destinationType, updateFunction);
        }

        #endregion

        #region Insert

        public static void InsertLoopBefore(Type sourceLoopType, Type destinationLoopType, PlayerLoopSystem.UpdateFunction updateFunction)
        {
            InsertLoopWithOffset(sourceLoopType, destinationLoopType, 0, updateFunction);
        }

        public static void InsertLoopBefore<TLoopSource, TLoopDestination>(PlayerLoopSystem.UpdateFunction updateFunction)
        {
            var sourceType = typeof(TLoopSource);
            var destinationType = typeof(TLoopDestination);
            InsertLoopBefore(sourceType, destinationType, updateFunction);
        }

        public static void InsertLoopAfter(Type sourceLoopType, Type destinationLoopType, PlayerLoopSystem.UpdateFunction updateFunction)
        {
            InsertLoopWithOffset(sourceLoopType, destinationLoopType, 1, updateFunction);
        }

        public static void InsertLoopAfter<TLoopSource, TLoopDestination>(PlayerLoopSystem.UpdateFunction updateFunction)
        {
            var sourceType = typeof(TLoopSource);
            var destinationType = typeof(TLoopDestination);
            InsertLoopAfter(sourceType, destinationType, updateFunction);
        }

        private static void InsertLoopWithOffset(Type sourceLoopType, Type destinationLoopType, int insertOffset, PlayerLoopSystem.UpdateFunction updateFunction)
        {
            if (updateFunction == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(updateFunction));
                return;
            }

            var currentSystem = PlayerLoop.GetCurrentPlayerLoop();
            ref var parentSystem = ref currentSystem.GetParentSystemRecursiveOf(sourceLoopType);

            var sourceSubIndex = parentSystem.FindSubSystemIndex(sourceLoopType);
            if (sourceSubIndex == -1)
            {
                var message = $"Not found {nameof(sourceLoopType)}({sourceLoopType.Name})";
                Debug.LogWarning(message);
                return;
            }

            var destinationSystem = new PlayerLoopSystem
            {
                type = destinationLoopType,
                updateDelegate = updateFunction
            };

            if (parentSystem.subSystemList == null)
            {
                parentSystem.subSystemList = new[] { destinationSystem };
            }
            else if (parentSystem.HasSubSystemOf(destinationLoopType))
            {
                var message = $"{nameof(sourceLoopType)}({sourceLoopType.Name}) already contains {nameof(destinationLoopType)}{destinationLoopType}";
                Debug.LogWarning(message);
                return;
            }
            else
            {
                var destinationIndex = sourceSubIndex + insertOffset;
                destinationIndex = Mathf.Clamp(destinationIndex, 0, parentSystem.subSystemList.Length);

                var subSystems = parentSystem.subSystemList.ToList();
                subSystems.Insert(destinationIndex, destinationSystem);
                parentSystem.subSystemList = subSystems.ToArray();
            }

            PlayerLoop.SetPlayerLoop(currentSystem);
        }

        #endregion

        #region Remove

        public static bool RemoveLoop(Type loopType)
        {
            var currentSystem = PlayerLoop.GetCurrentPlayerLoop();
            var anyRemoved = currentSystem.RemoveSubSystemRecursive(loopType);
            if (anyRemoved)
            {
                PlayerLoop.SetPlayerLoop(currentSystem);
            }

            return anyRemoved;
        }

        public static bool RemoveLoop<TLoop>()
        {
            var loopType = typeof(TLoop);
            return RemoveLoop(loopType);
        }

        #endregion

        #region Logging

        public static void LogCurrentPlayerLoopTypes(string message, LogType logType = LogType.Log)
        {
            var currentLoop = PlayerLoop.GetCurrentPlayerLoop();
            var loopTypes = currentLoop.GetTypes();

            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(message);
            stringBuilder.Append("Loops count: ");
            stringBuilder.AppendLine(loopTypes.Length);
            foreach (var loopType in loopTypes)
            {
                if (loopType == null)
                {
                    stringBuilder.AppendLine("Empty(null)");
                    continue;
                }

                stringBuilder.AppendLine(loopType.Name);
            }

            Debug.unityLogger.Log(logType, stringBuilder);
        }

        #endregion
    }
}