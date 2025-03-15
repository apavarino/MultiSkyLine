using System;
using System.IO;
using fastJSON;
using MSL.model;

namespace MSL
{
    public class Configs
    {
        private static readonly string ConfigFile = Path.Combine(Utils.GetModDirectory(), "configs.json");

        public static MslConfig LoadConfig()
        {
            MslLogger.LogWriteToDisk("Loading configs from " + ConfigFile + "...");
            try
            {
                var streamReader = new StreamReader(ConfigFile);
                var loadedConfig = JSON.ToObject<MslConfig>(streamReader.ReadToEnd());
                
                MslLogger.LogWriteToDisk("Finished loading configs ...");
                return loadedConfig;
            }
            catch (Exception e)
            {
                MslLogger.LogError("Could not load configs: " + e.Message);
                return new MslConfig();
            }
        }

        public static void SaveConfig(MslConfig config)
        {
            MslLogger.LogWriteToDisk("Savings configs to " + ConfigFile + "...");
            try
            {
                using (var streamWriter = new StreamWriter(ConfigFile))
                {
                    var jsonConfig = JSON.ToJSON(config);
                    streamWriter.Write(jsonConfig);
                }
            }
            catch (Exception e)
            {
                MslLogger.LogError("Could not save configs: " + e.Message); 
            }
        }
    }
}