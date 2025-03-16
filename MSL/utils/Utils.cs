using System.Linq;
using ColossalFramework.Plugins;

namespace MSL.utils
{
    public static class Utils
    {
        public static string GetModDirectory()
        {
            return (
                from plugin in PluginManager.instance.GetPluginsInfo()
                where plugin?.userModInstance != null && plugin.userModInstance.GetType().Namespace == "MSL"
                select plugin.modPath
            ).FirstOrDefault();
        }
    }
}