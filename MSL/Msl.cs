using ColossalFramework.UI;
using ICities;
using MSL.client;
using MSL.client.ui;
using MSL.model;
using MSL.server;
using UnityEngine;

namespace MSL
{
    public class Msl : IUserMod, ILoadingExtension
    {
        public string Name => "MSL";
        public string Description => "MultiSkyLine";

        private EmbeddedServer _server;

        private CityDataEmitter _cityDataEmitter;
        private CityDataFetcher _cityDataFetcher;
        
        private static readonly string DefaultServerIP = "127.0.0.1";
        public static string ServerIP = DefaultServerIP;
        private static bool _isServerEnabled = true;
        
        public static MslConfig Config = new MslConfig();

        private GameObject _uiObject;
        
        private readonly Configs _configs = new Configs();
        
        public void OnEnabled()
        {
            Config = Configs.LoadConfig();
            _isServerEnabled = Config.IsServerEnabled;
            if (!_isServerEnabled)
            {
                ServerIP = Config.ServerURL;
            }
        }

        public void OnDisabled()
        {
            Configs.SaveConfig(Config);
        }

        public void OnLevelLoaded(LoadMode mode)
        {
            if (_isServerEnabled)
            {
                ServerIP = DefaultServerIP;
                StartServer();
            }
            else
            {
                ServerIP = Config.ServerURL;
            }

            MslLogger.LogSuccess($"Mod enable. Server active : {_isServerEnabled}");
            
            _cityDataEmitter = new CityDataEmitter();
            _cityDataFetcher = new CityDataFetcher();
            _cityDataEmitter.Start();
            _cityDataFetcher.Start();

            _uiObject = new GameObject("CityDataUI");
            _uiObject.AddComponent<CityDataUI>();
        }

        public void OnLevelUnloading()
        {
            if (_uiObject != null)
            {
                GameObject.Destroy(_uiObject);
            }
            
            _cityDataEmitter?.Stop();
            _cityDataFetcher?.Stop();
            StopServer();
            Configs.SaveConfig(Config);
            MslLogger.LogStop("Mod disabled");
        }
        

        private void StartServer()
        {
            _server = new EmbeddedServer(ServerIP);
            _server.Start();
        }

        private void StopServer()
        {
            _server?.Stop();
        }

        // Interface de configuration
        public void OnSettingsUI(UIHelperBase helper)
        {
            var group = helper.AddGroup("Paramètres du Serveur");
            UITextField textfield = null;

            // Ajouter une case à cocher pour activer/désactiver le serveur
            group.AddCheckbox("Start server", _isServerEnabled, (value) =>
            {
                _isServerEnabled = value;
                MslLogger.LogServer($"Server state changed : {_isServerEnabled}");

                MslLogger.LogServer("updating the config file for restarting the server correctly");
                Config.IsServerEnabled = _isServerEnabled;
                if (_isServerEnabled)
                {
                    MslLogger.LogServer("Updating the server ip to local url");
                    ServerIP = DefaultServerIP;
                    StartServer();
                }
                else
                {
                    MslLogger.LogServer("Updating the server ip to distant url");
                    ServerIP = Config.ServerURL;
                    StopServer();
                }

                if (textfield != null)
                {
                    textfield.text = ServerIP;
                }
            });

            // Ajouter un champ texte pour l'IP
            textfield = group.AddTextfield("Server address", ServerIP, (value) =>
            {
                ServerIP = value;

                if (!_isServerEnabled)
                {
                    Config.ServerURL = ServerIP;
                }
                
                MslLogger.LogServer($"New IP : {ServerIP}");
            }) as UITextField;
            
        }
        
        public void OnCreated(ILoading loading)
        {
        }

        public void OnReleased()
        {
        }
    }
    
}