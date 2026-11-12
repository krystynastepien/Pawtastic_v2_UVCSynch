using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class GameLogger
{
    private static LoggerConfig _config;
    private static readonly char[] PathSeparators = { '/', '\\' };

    private static readonly string _CRITICAL = "CRITICAL";
    private static readonly string _ERROR = "ERROR";
    private static readonly string _WARNING = "WARNING";
    private static readonly string _INFO = "INFO";

    private static LoggerConfig Config
    {
        get
        {
            if (_config == null)
            {
                _config = Resources.Load<LoggerConfig>("System_Prefabs/LoggerConfig");
                if (_config == null)
                {
                    Debug.LogWarning("LoggerConfig prefab not found in Resources folder.");
                }
            }
            return _config;
        }
    }

    #region Log Methods
        /// <summary>
        /// Logs an info message if its category is enabled in LoggerConfig
        /// Result: [INFO] [System] [Singleton.cs - Initialize]           Created manager: GameManager
        /// </summary>
        /// <param name="message">Log message</param>
        /// <param name="category">System the message belongs to (UI, Player, etc.)</param>
        /// <param name="context">Object highlighted in the hierarchy when the log is clicked</param>  
        public static void Log(
                                string message, 
                                LoggerConfig.LogCategoryFlags category, 
                                Object context = null,
                                [CallerMemberName] string member = null, 
                                [CallerFilePath] string file = null
                            )
        {
            if (Config == null || (Config.enabledCategories & category) == 0) return;
            Debug.Log(Format(message, category, member, file, _INFO), context);
        }


        /// <summary>
        /// Logs an info message if its category is enabled in LoggerConfig
        /// Result: [WARNING] [UI] [HUDManager.cs - LogMissingObject]         HUD is missing object: InventoryController
        /// </summary>
        /// <param name="message">Warning message</param>
        /// <param name="category">System the message belongs to (UI, Player, etc.)</param>
        /// <param name="context">Object highlighted in the hierarchy when the log is clicked</param>  
        public static void LogWarning(
                                string message, 
                                LoggerConfig.LogCategoryFlags category, 
                                Object context = null,
                                [CallerMemberName] string member = null, 
                                [CallerFilePath] string file = null
                            )
        {
            if (Config == null || (Config.enabledCategories & category) == 0) return;
            Debug.LogWarning(Format(message, category, member, file, _WARNING), context);
        }


        /// <summary>
        /// Logs an info message if its category is enabled in LoggerConfig
        /// Result: [ERROR][UI] [HUDManager.cs - LogMissingObject]      HUD is missing object: InventoryController
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="category">System the message belongs to (UI, Player, etc.)</param>
        /// <param name="context">Object highlighted in the hierarchy when the log is clicked</param>  
        public static void LogError(
                                string message, 
                                LoggerConfig.LogCategoryFlags category, 
                                Object context = null,
                                [CallerMemberName] string member = null, 
                                [CallerFilePath] string file = null
                            )
        {
            if (Config == null || (Config.enabledCategories & category) == 0) return;
            Debug.LogError(Format(message, category, member, file, _ERROR), context);
        }


                /// <summary>
        /// Logs an info message if its category is enabled in LoggerConfig
        /// Result: [CRITICAL] [UI] [HUDManager.cs - LogMissingObject]      HUD is missing object: InventoryController
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="category">System the message belongs to (UI, Player, etc.)</param>
        /// <param name="context">Object highlighted in the hierarchy when the log is clicked</param>  
        public static void LogCritical(
                                string message, 
                                LoggerConfig.LogCategoryFlags category, 
                                Object context = null,
                                [CallerMemberName] string member = null, 
                                [CallerFilePath] string file = null
                            )
        {
            if (Config == null || (Config.enabledCategories & category) == 0) return;
            Debug.LogError(Format(message, category, member, file, _CRITICAL), context);
        }

    #endregion

    #region Helpers
        /// <summary>
        ///   Result: [WARNING] [UI] [HUDManager.cs - OnStart]       HUD is missing InventoryController
        /// </summary>
        /// <param name="message">Log message</param>
        /// <param name="category">System the message belongs to (UI, Player, etc.)</param>
        /// <param name="method">what method was it called from</param>
        /// <param name="file">what file was it called from</param>
        /// <returns></returns>
        private static string Format(string message, LoggerConfig.LogCategoryFlags category, string method, string file, string type)
        {
            string msg = $"[{type}]  [{category}]  [{Path.GetFileName(file)} - {method}]      {message}";
            return msg;
        }
    
    #endregion
}
