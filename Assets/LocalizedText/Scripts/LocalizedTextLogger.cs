using System;
using System.Diagnostics;
using UnityEngine;

namespace LocalText.Internal
{
    public static class LocalizedTextLogger
    {
        public enum LogLevel
        {
            None = 0,
            Info = 1,
            Debug = 2,
            Verbose = 3,
        }

        public static LogLevel CurrentLogLevel { get; set; }

        public static ILogger logger { private get; set; }

        static LocalizedTextLogger()
        {
            logger = UnityEngine.Debug.logger;
        }

        [Conditional("UNITY_EDITOR")]
        public static void Log(LogLevel logLevel, string message)
        {
            if(logLevel < CurrentLogLevel)
            {
                return;
            }

            logger.Log(LogType.Log, message);
        }

        [Conditional("UNITY_EDITOR")]
        public static void LogFormat(LogLevel logLevel, string message, params object[] args)
        {
            if(logLevel < CurrentLogLevel)
            {
                return;
            }

            logger.LogFormat(LogType.Log, message, args);
        }

        [Conditional("UNITY_EDITOR")]
        public static void Assert(Action<bool> condition, string message = "")
        {
            logger.Log(LogType.Assert, message);
        }
    }
}
