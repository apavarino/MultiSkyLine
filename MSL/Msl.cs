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


        public void OnEnabled()
        {
            if (_isServerEnabled)
            {
                StartServer();
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

            // Ajouter une case à cocher pour activer/désactiver le serveur
            group.AddCheckbox("Start server", _isServerEnabled, (value) =>
            {
                _isServerEnabled = value;
                MslLogger.LogServer($"Server state changed : {_isServerEnabled}");

                if (_isServerEnabled)
                {
                    StartServer();
                }
                else
                {
                    StopServer();
                }
            });

            // Ajouter un champ texte pour l'IP
            group.AddTextfield("Server address", ServerIP, (value) =>
            {
                ServerIP = value;
                MslLogger.LogServer($"New IP : {ServerIP}");
            });
        }
    }
}