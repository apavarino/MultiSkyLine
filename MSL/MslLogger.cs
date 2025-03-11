using System;
using System.IO;

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
        public const string WriteToDisk= "🔏";
    }
    
    public static class MslLogger
    {
        private static readonly string LogFilePath = Path.Combine(Utils.GetModDirectory(), "client.log");
        private static readonly string ServerLogFilePath = Path.Combine(Utils.GetModDirectory(), "server.log");

        // Server
        public static void LogServer(string message)
        {
            LogServer(message, LogState.Server);
        }
        
        // Client
        public static void LogError(string message)
        {
            LogClient(message, LogState.Error);
        }
        
        public static void LogSuccess(string message)
        {
            LogClient(message, LogState.Success);
        }
        
        public static void LogWarn(string message)
        {
            LogClient(message, LogState.Warning);
        }
        
        public static void LogStart(string message)
        {
            LogClient(message, LogState.Start);
        }
        
        public static void LogStop(string message)
        {
            LogClient(message, LogState.Stop);
        }
        
        public static void LogSend(string message)
        {
            LogClient(message, LogState.Send);
        }

        public static void LogWriteToDisk(string message)
        {
            LogClient(message, LogState.WriteToDisk);
        }

        private static void LogServer(string message, String logState)
        { 
            Log(message,logState,ServerLogFilePath);
        }

        private static void LogClient(string message, String logState)
        {
           Log(message,logState,LogFilePath);
        }

        private static void Log(string message, String logState, String logPath)
        {
            try
            {
                using (var writer = new StreamWriter(logPath, true))
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
    }
}