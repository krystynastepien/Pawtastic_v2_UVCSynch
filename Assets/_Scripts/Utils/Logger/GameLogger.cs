using UnityEngine;

public static class GameLogger
{
    private static LoggerConfig _config;

    private static LoggerConfig Config
    {
        get
        {
            if (_config == null)
            {
                _config = Resources.Load<LoggerConfig>("LoggerConfig");
                if (_config == null)
                {
                    Debug.LogWarning("LoggerConfig not found in Resources folder. Creating default settings.");
                }
            }
            return _config;
        }
    }

    #region Log Methods
    public static void Log(string message, LoggerConfig.LogCategoryFlags category, Object context = null)
    {
        if (Config != null && (Config.enabledCategories & category) != 0)
        {
            Debug.Log($"[{category}] {message}", context);
        }
    }

    public static void LogWarning(string message, LoggerConfig.LogCategoryFlags category, Object context = null)
    {
        if (Config != null && Config.enableWarnings && (Config.enabledCategories & category) != 0)
        {
            Debug.LogWarning($"[{category}] {message}", context);
        }
    }

    public static void LogError(string message, LoggerConfig.LogCategoryFlags category, Object context = null)
    {
        if (Config != null && Config.enableErrors && (Config.enabledCategories & category) != 0)
        {
            Debug.LogError($"[{category}] {message}", context);
        }
    }

    // Convenience methods without category (always logs if general logging enabled)
    public static void Log(string message, Object context = null)
    {
        Debug.Log(message, context);
    }

    public static void LogWarning(string message, Object context = null)
    {
        if (Config != null && Config.enableWarnings)
        {
            Debug.LogWarning(message, context);
        }
    }

    public static void LogError(string message, Object context = null)
    {
        if (Config != null && Config.enableErrors)
        {
            Debug.LogError(message, context);
        }
    }
    #endregion
}