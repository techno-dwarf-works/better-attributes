using System;
using System.Diagnostics;
using Better.Commons.Runtime.Extensions;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace Better.Commons.Runtime.Utility
{
    public static class DebugUtility
    {
        [DebuggerHidden]
        [DebuggerNonUserCode]
        public static void LogException<T>()
            where T : Exception, new()
        {
            var exception = new T();
            Debug.LogException(exception);
        }

        [DebuggerHidden]
        [DebuggerNonUserCode]
        public static void LogException<T>(Object context)
            where T : Exception, new()
        {
            var exception = new T();
            Debug.LogException(exception, context);
        }

        [DebuggerHidden]
        [DebuggerNonUserCode]
        public static void LogException<T>(string message)
            where T : Exception, new()
        {
            var exception = new T();
            exception.ReplaceExceptionMessageField(message);
            Debug.LogException(exception);
        }

        [DebuggerHidden]
        [DebuggerNonUserCode]
        public static void LogException<T>(object message)
            where T : Exception, new()
        {
            LogException<T>(message.ToString());
        }

        [DebuggerHidden]
        [DebuggerNonUserCode]
        public static void LogException<T>(string message, Object context)
            where T : Exception, new()
        {
            var exception = new T();
            exception.ReplaceExceptionMessageField(message);
            Debug.LogException(exception, context);
        }

        [DebuggerHidden]
        [DebuggerNonUserCode]
        public static void LogException<T>(object message, Object context)
            where T : Exception, new()
        {
            LogException<T>(message.ToString(), context);
        }

        [DebuggerHidden]
        [DebuggerNonUserCode]
        public static void LogException(string message)
        {
            var exception = new Exception(message);
            Debug.LogException(exception);
        }

        [DebuggerHidden]
        [DebuggerNonUserCode]
        public static void LogException(object message)
        {
            LogException(message.ToString());
        }

        [DebuggerHidden]
        [DebuggerNonUserCode]
        public static void LogException(string message, Object context)
        {
            var exception = new Exception(message);
            Debug.LogException(exception, context);
        }

        [DebuggerHidden]
        [DebuggerNonUserCode]
        public static void LogException(object message, Object context)
        {
            LogException(message.ToString(), context);
        }
    }
}