using System;
using System.IO;
using fastJSON;
using MSL.model;

namespace MSL
{
    public class Configs
    {
        private static readonly string ConfigFile = Path.Combine(Utils.GetModDirectory(), "configs.json");

        public static MSLConfig LoadConfig()
        {
            MslLogger.LogWriteToDisk("Loading configs from " + ConfigFile + "...");
            try
            {
                var sr = new StreamReader(ConfigFile);
                var tmp = JSON.ToObject<MSLConfig>(sr.ReadToEnd());
                
                MslLogger.LogWriteToDisk("Finished loading configs ...");
                return tmp;
            }
            catch (Exception e)
            {
                MslLogger.LogError("Could not load configs: " + e.Message);
                return new MSLConfig();
            }
        }

        public static void SaveConfig(MSLConfig config)
        {
            MslLogger.LogWriteToDisk("Savings configs to " + ConfigFile + "...");
            try
            {
                using (var sw = new StreamWriter(ConfigFile))
                {
                    var jsonStr = JSON.ToJSON(config);
                    sw.Write(jsonStr);
                }
            }
            catch (Exception e)
            {
                MslLogger.LogError("Could not save configs: " + e.Message); 
            }
        }
    }
}