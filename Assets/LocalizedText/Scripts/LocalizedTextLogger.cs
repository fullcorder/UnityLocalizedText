using System;
using System.Diagnostics;
using UnityEngine;

namespace LocalText.Internal
{
    public static class LocalizedTextLogger
    {
        private static readonly string TAG = "LocalizedText";

        public enum LogLevel
        {
            None = 0,
            Info = 1,
            Verbose = 2,
        }

        public static LogLevel CurrentLogLevel { get; set; }

        private static ILogger logger { get; set; }

        static LocalizedTextLogger()
        {
            logger = UnityEngine.Debug.logger;
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void Info(string message)
        {
            Log(LogLevel.Info, message);
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void InfoFormat(string message, params object[] args)
        {
            LogFormat(LogLevel.Info, message, args);
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void Verbose(string message)
        {
            Log(LogLevel.Verbose, message);
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void VerboseFormat(string message, params object[] args)
        {
            LogFormat(LogLevel.Verbose, message, args);
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void Error(string message)
        {
            logger.LogError(TAG, message);
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void ErrorFormat(string message, params object[] args)
        {
            logger.LogError(TAG, string.Format(message, args));
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        private static void Log(LogLevel logLevel, string message)
        {
            if(logLevel < CurrentLogLevel)
            {
                return;
            }

            logger.Log(LogType.Log, TAG, message);
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        private static void LogFormat(LogLevel logLevel, string message, params object[] args)
        {
            if(logLevel < CurrentLogLevel)
            {
                return;
            }

            logger.LogFormat(LogType.Log, TAG, message, args);
        }
    }
}
