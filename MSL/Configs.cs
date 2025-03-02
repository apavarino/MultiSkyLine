using System;
using System.IO;
using fastJSON;

namespace MSL
{
    public class Configs
    {
        private static readonly String ConfigFile = Path.Combine(Utils.GetModDirectory(), "configs.json");

        public String LocalUrl { get; set; } = "127.0.0.1";
        public String DistantUrl { get; set; } = "127.0.0.1";
        public Boolean IsServerEnabled { get; set; } = true;


        public void LoadConfig()
        {
            MslLogger.LogWriteToDisk("Loading configs from " + ConfigFile + "...");
            try
            {
                var sr = new StreamReader(ConfigFile);
                var tmp = JSON.ToObject<Configs>(sr.ReadToEnd());
                LocalUrl = tmp.LocalUrl;
                DistantUrl = tmp.DistantUrl;
                IsServerEnabled = tmp.IsServerEnabled;
                
                MslLogger.LogWriteToDisk("Finished loading configs ...");
            }
            catch (Exception e)
            {
                MslLogger.LogWarn("Could not load configs: " + e.Message);
            }
        }

        public void SaveConfig()
        {
            MslLogger.LogWriteToDisk("Savings configs to " + ConfigFile + "...");
            try
            {
                using (var sw = new StreamWriter(ConfigFile))
                {
                    var jsonStr = JSON.ToJSON(this);
                    sw.Write(jsonStr);
                }
            }
            catch (Exception e)
            {
                MslLogger.LogWarn("Could not save configs: " + e.Message); 
            }
        }
    }
}