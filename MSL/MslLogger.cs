using System;
using System.IO;
using System.Linq;
using ColossalFramework.Plugins;

namespace MSL
{
    static class LogState
    {
        public const string Server = "🌍";
        public const string Error = "❌";
        public const string Success = "✅";
        public const string Warning = "⚠️️";
        public const string Start = "🚀";
        public const string Stop = "⛔️";
        public const string Send= "📦";
    }
    
    public static class MslLogger
    {
        private static readonly string LogFilePath = Path.Combine(GetModDirectory(), "msl.log");

        public static void LogError(string message)
        {
            Log(message, LogState.Error);
        }
        
        public static void LogServer(string message)
        {
            Log(message, LogState.Server);
        }
        
        public static void LogSuccess(string message)
        {
            Log(message, LogState.Success);
        }
        
        public static void LogWarn(string message)
        {
            Log(message, LogState.Warning);
        }
        
        public static void LogStart(string message)
        {
            Log(message, LogState.Start);
        }
        
        public static void LogStop(string message)
        {
            Log(message, LogState.Stop);
        }
        
        public static void LogSend(string message)
        {
            Log(message, LogState.Send);
        }

        public static void Log(string message, String logState)
        {
            try
            {
                using (var writer = new StreamWriter(LogFilePath, true))
                {
                    if (message == null) return;
                    var logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {logState} {message}";
                    writer.WriteLine(logMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Logging error : {ex.Message}");
            }
        }

        private static string GetModDirectory()
        {
            return (
                from plugin in PluginManager.instance.GetPluginsInfo()
                where plugin?.userModInstance != null && plugin.userModInstance.GetType().Namespace == "MSL"
                select plugin.modPath
            ).FirstOrDefault();
        }
    }
}