using UnityEngine;
using UnityEditor;

namespace Core.Logging
{
    public enum LogColor
    {
        White, Black, Red, Green, Blue, Yellow, Orange
    }

    public class CLogger
    {
        public static bool loggingEnabled = true;

        readonly bool _enabledOnInstance;

        [MenuItem("CLogger/Logs Enabled", priority = 1)]
        static void ToggleLoggingEnabled()
        {
            loggingEnabled = !loggingEnabled;
            Menu.SetChecked("CLogger/Logs Enabled", loggingEnabled);
        }

        public CLogger(bool enabledOnInstance)
        {
            if (!loggingEnabled)
            {
                _enabledOnInstance = false;
                return;
            }

            _enabledOnInstance = enabledOnInstance;
        }

        public void Log(string message, LogColor color = LogColor.White)
        {
            #if UNITY_EDITOR

            if (!_enabledOnInstance) return;
            Debug.Log(ColorString(message, color));

            #endif
        }
        public void Log(string message, GameObject obj, LogColor color = LogColor.White)
        {
            #if UNITY_EDITOR

            if (!_enabledOnInstance) return;
            Debug.Log(ColorString(message, color), obj);

            #endif
        }

        public void LogError(string message, LogColor color = LogColor.White)
        {
            #if UNITY_EDITOR

            if (!_enabledOnInstance) return;
            Debug.LogError(ColorString(message, color));

            #endif
        }
        public void LogError(string message, GameObject obj, LogColor color = LogColor.White)
        {
            #if UNITY_EDITOR

            if (!_enabledOnInstance) return;
            Debug.LogError(ColorString(message, color), obj);

            #endif
        }

        public void LogWarning(string message, LogColor color = LogColor.White)
        {
            #if UNITY_EDITOR

            if (!_enabledOnInstance) return;
            Debug.LogWarning(ColorString(message, color));

            #endif
        }
        public void LogWarning(string message, GameObject obj, LogColor color = LogColor.White)
        {
            #if UNITY_EDITOR

            if (!_enabledOnInstance) return;
            Debug.LogWarning(ColorString(message, color), obj);

            #endif
        }

        static string ColorString(string message, LogColor color)
        {
            string stringColor = color.ToString().ToLower();
            string coloredMessage = $"<color={stringColor}>{message}</color>";
            return coloredMessage;
        }
    }
}
