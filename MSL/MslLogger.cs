using System;
using System.IO;
using System.Linq;
using ColossalFramework.Plugins;

namespace MSL
{
    public static class MslLogger
    {
        private static readonly string LogFilePath = Path.Combine(GetModDirectory(), "msl.log");

        public static void Log(string message)
        {
            try
            {
                using (var writer = new StreamWriter(LogFilePath, true))
                {
                    if (message == null) return;
                    var logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";
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