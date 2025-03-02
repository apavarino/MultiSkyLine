using ColossalFramework.UI;
using ICities;
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
        
        public static string ServerIP = "127.0.0.1";
        private static bool _isServerEnabled = true;

        private GameObject _uiObject;
        
        private readonly Configs _configs = new Configs();


        public void OnEnabled()
        {
            _configs.LoadConfig();
            _isServerEnabled = _configs.IsServerEnabled;
            
            if (_isServerEnabled)
            {
                ServerIP = _configs.LocalUrl;
                StartServer();
            }
            else
            {
                ServerIP = _configs.DistantUrl;
            }

            MslLogger.LogSuccess($"Mod enable. Server active : {_isServerEnabled}");
        }

        public void OnCreated(ILoading loading)
        {
        }

        public void OnReleased()
        {
        }

        public void OnLevelLoaded(LoadMode mode)
        {
            _cityDataEmitter = new CityDataEmitter();
            _cityDataFetcher = new CityDataFetcher();
            _cityDataEmitter.Start();
            _cityDataFetcher.Start();

            _uiObject = new GameObject("CityDataUI");
            _uiObject.AddComponent<CityDataUI>();
        }

        public void OnLevelUnloading()
        {
        }

        public void OnDisabled()
        {
            if (_uiObject != null)
            {
                GameObject.Destroy(_uiObject);
            }
            
            _cityDataEmitter?.Stop();
            _cityDataFetcher?.Stop();
            StopServer();
            _configs.SaveConfig();
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
                _configs.IsServerEnabled = _isServerEnabled;
                if (_isServerEnabled)
                {
                    MslLogger.LogServer("Updating the server ip to local url");
                    ServerIP = _configs.LocalUrl;
                    StartServer();
                }
                else
                {
                    MslLogger.LogServer("Updating the server ip to distant url");
                    ServerIP = _configs.DistantUrl;
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

                if (_isServerEnabled)
                {
                    _configs.LocalUrl = ServerIP;
                }
                else
                {
                    _configs.DistantUrl = ServerIP;
                }
                
                MslLogger.LogServer($"New IP : {ServerIP}");
            }) as UITextField;
            
        }
    }
}