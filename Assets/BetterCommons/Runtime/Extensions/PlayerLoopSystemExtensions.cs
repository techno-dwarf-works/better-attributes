using System;
using System.Collections.Generic;
using System.Linq;
using Better.Commons.Runtime.Utility;
using UnityEngine.LowLevel;

namespace Better.Commons.Runtime.Extensions
{
    public static class PlayerLoopSystemExtensions
    {
        public static bool HasSubSystemOf(this ref PlayerLoopSystem self, Type subSystemType)
        {
            var index = self.FindSubSystemIndex(subSystemType);
            return index != -1;
        }

        public static bool HasSubSystemOf<T>(this ref PlayerLoopSystem self)
        {
            var subSystemType = typeof(T);
            return self.HasSubSystemOf(subSystemType);
        }

        public static int FindSubSystemIndex(this ref PlayerLoopSystem self, Type subSystemType)
        {
            var subSystems = self.subSystemList;
            if (subSystems == null)
            {
                return -1;
            }

            for (int i = 0; i < subSystems.Length; i++)
            {
                if (subSystems[i].type == subSystemType)
                {
                    return i;
                }
            }

            return -1;
        }

        public static int FindSubSystemIndex<T>(this ref PlayerLoopSystem self)
        {
            var subSystemType = typeof(T);
            return self.FindSubSystemIndex(subSystemType);
        }

        public static ref PlayerLoopSystem GetSubSystem(this ref PlayerLoopSystem self, Type subSystemType)
        {
            var index = self.FindSubSystemIndex(subSystemType);
            if (index == -1)
            {
                return ref self;
            }

            return ref self.subSystemList[index];
        }

        public static ref PlayerLoopSystem GetSubSystem<TSubSystem>(this ref PlayerLoopSystem self)
        {
            var subSystemType = typeof(TSubSystem);
            return ref self.GetSubSystem(subSystemType);
        }

        public static ref PlayerLoopSystem GetSubSystemRecursive(this ref PlayerLoopSystem self, Type subSystemType)
        {
            var subSystems = self.subSystemList;
            if (subSystems == null)
            {
                return ref self;
            }

            ref var subSystem = ref self.GetSubSystem(subSystemType);
            if (subSystem.type == subSystemType)
            {
                return ref subSystem;
            }

            for (int i = 0; i < subSystems.Length; i++)
            {
                ref var buffer = ref GetSubSystemRecursive(ref subSystems[i], subSystemType);
                if (buffer.type == subSystemType)
                {
                    return ref buffer;
                }
            }

            return ref self;
        }

        public static ref PlayerLoopSystem GetSubSystemRecursive<TSubSystem>(this ref PlayerLoopSystem self)
        {
            var subSystemType = typeof(TSubSystem);
            return ref self.GetSubSystemRecursive(subSystemType);
        }

        public static ref PlayerLoopSystem GetParentSystemRecursiveOf(this ref PlayerLoopSystem self, Type subSystemType)
        {
            if (self.type == subSystemType)
            {
                return ref self;
            }

            var subSystems = self.subSystemList;
            if (subSystems == null)
            {
                return ref self;
            }

            if (self.HasSubSystemOf(subSystemType))
            {
                return ref self;
            }

            for (int i = 0; i < subSystems.Length; i++)
            {
                ref var buffer = ref subSystems[i];
                buffer = ref buffer.GetParentSystemRecursiveOf(subSystemType);
                if (buffer.HasSubSystemOf(subSystemType))
                {
                    return ref buffer;
                }
            }

            return ref self;
        }

        public static ref PlayerLoopSystem GetParentSystemRecursiveOf<TSubSystem>(this ref PlayerLoopSystem self)
        {
            var subSystemType = typeof(TSubSystem);
            return ref self.GetParentSystemRecursiveOf(subSystemType);
        }

        public static bool RemoveSubSystem(this ref PlayerLoopSystem self, Type subSystemType)
        {
            if (self.subSystemList == null)
            {
                return false;
            }

            var anyRemoved = false;
            var subSystems = self.subSystemList.ToList();
            for (var i = subSystems.Count - 1; i >= 0; i--)
            {
                if (subSystems[i].type == subSystemType)
                {
                    subSystems.RemoveAt(i);
                    anyRemoved = true;
                }
            }

            if (anyRemoved)
            {
                self.subSystemList = subSystems.ToArray();
            }

            return anyRemoved;
        }

        public static bool RemoveSubSystem<TSubSystem>(this ref PlayerLoopSystem self)
        {
            var subSystemType = typeof(TSubSystem);
            return self.RemoveSubSystem(subSystemType);
        }

        public static bool RemoveSubSystemRecursive(this ref PlayerLoopSystem self, Type subSystemType)
        {
            var removeAny = self.RemoveSubSystem(subSystemType);
            var subSystems = self.subSystemList;
            if (subSystems != null)
            {
                for (int i = 0; i < subSystems.Length; i++)
                {
                    if (subSystems[i].RemoveSubSystemRecursive(subSystemType))
                    {
                        removeAny = true;
                    }
                }
            }

            return removeAny;
        }

        public static bool RemoveSubSystemRecursive<TSubSystem>(this ref PlayerLoopSystem self)
        {
            var subSystemType = typeof(TSubSystem);
            return self.RemoveSubSystemRecursive(subSystemType);
        }

        public static bool UnsubscribeRecursive(this ref PlayerLoopSystem self, PlayerLoopSystem.UpdateFunction updateFunction)
        {
            if (updateFunction == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(updateFunction));
                return false;
            }

            var result = false;
#if UNITY_EDITOR
            var invocationList = self.updateDelegate.GetInvocationList();
            if (invocationList.Contains(updateFunction))
            {
                self.updateDelegate -= updateFunction;
                result = true;
            }
#else
                    loopSystem.updateDelegate -= updateFunction;
                    result = true;
#endif

            var subSystems = self.subSystemList;
            if (subSystems != null)
            {
                for (int i = 0; i < subSystems.Length; i++)
                {
                    if (subSystems[i].UnsubscribeRecursive(updateFunction))
                    {
                        result = true;
                    }
                }
            }

            return result;
        }

        public static bool UnsubscribeRecursive(this ref PlayerLoopSystem self, Type loopType, PlayerLoopSystem.UpdateFunction updateFunction)
        {
            var result = false;
            if (self.type == loopType)
            {
#if UNITY_EDITOR
                var invocationList = self.updateDelegate.GetInvocationList();
                if (invocationList.Contains(updateFunction))
                {
                    self.updateDelegate -= updateFunction;
                    result = true;
                }
#else
                    loopSystem.updateDelegate -= updateFunction;
                    result = true;
#endif
            }

            var subSystems = self.subSystemList;
            if (subSystems != null)
            {
                for (int i = 0; i < subSystems.Length; i++)
                {
                    if (subSystems[i].UnsubscribeRecursive(loopType, updateFunction))
                    {
                        result = true;
                    }
                }
            }

            return result;
        }

        public static bool UnsubscribeRecursive<TLoop>(this ref PlayerLoopSystem self, PlayerLoopSystem.UpdateFunction updateFunction)
        {
            var loopType = typeof(TLoop);
            return self.UnsubscribeRecursive(loopType, updateFunction);
        }

        public static Type[] GetTypes(this ref PlayerLoopSystem self)
        {
            var loopTypes = new List<Type>();
            self.CollectTypesRecursive(ref loopTypes);
            return loopTypes.ToArray();
        }

        private static void CollectTypesRecursive(this ref PlayerLoopSystem self, ref List<Type> loopTypes)
        {
            loopTypes.Add(self.type);

            var subSystems = self.subSystemList;
            if (subSystems == null) return;

            for (int i = 0; i < subSystems.Length; i++)
            {
                subSystems[i].CollectTypesRecursive(ref loopTypes);
            }
        }
    }
}