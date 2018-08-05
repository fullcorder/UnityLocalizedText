using System;
using System.Diagnostics;
using UnityEngine;
using Object = System.Object;

namespace LocalizedText.Internal
{
    public static class LocalTextLogger
    {
        private static readonly string Tag = "LocalizedText";

        public enum LogLevel
        {
            None = 0,
            Info = 1,
            Verbose = 2,
        }

        public static LogLevel CurrentLogLevel { get; set; }

        private static ILogger Logger { get; set; }

        static LocalTextLogger()
        {
#if UNITY_5
            logger = UnityEngine.Debug.logger;
#else
            Logger = UnityEngine.Debug.unityLogger;
#endif
            CurrentLogLevel = LogLevel.Info;
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void Info(string message, params object[] args)
        {
            Log(LogLevel.Info, message, args);
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void Verbose(string message, params object[] args)
        {
            Log(LogLevel.Verbose, message, args);
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void Error(string message, params  object[] args)
        {
            Logger.LogError(Tag, string.Format(message, args));
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        private static void Log(LogLevel logLevel, string message, params object[] args)
        {
            if(logLevel < CurrentLogLevel)return;

            Logger.Log(LogType.Log, Tag, string.Format(message, args));
        }
    }
}
