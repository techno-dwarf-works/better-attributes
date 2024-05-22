using System;
using System.Reflection;
using Better.Commons.Runtime.Utility;

namespace Better.Commons.Runtime.Extensions
{
    public static class ExceptionExtensions
    {
        private const string ExceptionMessageFieldName = "_message";
        private static readonly FieldInfo _messageField;

        static ExceptionExtensions()
        {
            _messageField = GetExceptionMessageField();
        }

        private static FieldInfo GetExceptionMessageField()
        {
            var type = typeof(Exception);
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            return type.GetField(ExceptionMessageFieldName, flags);
        }

        public static void ReplaceExceptionMessageField(this Exception self, string message)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return;
            }

            _messageField.SetValue(self, message);
        }

        public static void ReplaceExceptionMessageField(this Exception self, object message)
        {
            if (message == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(message));
                return;
            }

            self.ReplaceExceptionMessageField(message.ToString());
        }
    }
}