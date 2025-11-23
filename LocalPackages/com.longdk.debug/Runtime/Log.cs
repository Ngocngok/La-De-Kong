using System.Diagnostics;
using UnityEngine;

namespace LongDK.Debug
{
    /// <summary>
    /// A wrapper around UnityEngine.Debug that supports conditional compilation.
    /// Calls to these methods will be stripped from the build unless LONGDK_DEBUG is defined.
    /// </summary>
    public static class Log
    {
        private const string SYMBOL = "LONGDK_DEBUG";

        [Conditional(SYMBOL)]
        public static void Msg(object message)
        {
            UnityEngine.Debug.Log($"[LongDK] {message}");
        }

        [Conditional(SYMBOL)]
        public static void Msg(object message, Object context)
        {
            UnityEngine.Debug.Log($"[LongDK] {message}", context);
        }

        [Conditional(SYMBOL)]
        public static void Warn(object message)
        {
            UnityEngine.Debug.LogWarning($"[LongDK] {message}");
        }

        [Conditional(SYMBOL)]
        public static void Warn(object message, Object context)
        {
            UnityEngine.Debug.LogWarning($"[LongDK] {message}", context);
        }

        [Conditional(SYMBOL)]
        public static void Error(object message)
        {
            UnityEngine.Debug.LogError($"[LongDK] {message}");
        }

        [Conditional(SYMBOL)]
        public static void Error(object message, Object context)
        {
            UnityEngine.Debug.LogError($"[LongDK] {message}", context);
        }
    }
}