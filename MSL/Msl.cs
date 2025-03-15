using ColossalFramework.UI;
using ICities;
using MSL.client.controller;
using MSL.client.ui;
using MSL.model.repository;
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
        private CityDataRepository _clientRepository;
        
        private static readonly string DefaultServerIP = "127.0.0.1";
        public static string ServerIP = DefaultServerIP;
        private static bool _isServerEnabled = true;
        
        public static MslConfig Config = new MslConfig();

        private CityDataUI _cityDataUI;
        
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

           
            _clientRepository = new CityDataRepository(SimulationManager.instance.m_metaData.m_CityName);
            _cityDataEmitter = new CityDataEmitter(_clientRepository);
            _cityDataFetcher = new CityDataFetcher(_clientRepository);
            _cityDataEmitter.Start();
            _cityDataFetcher.Start();
            
            _cityDataUI = new GameObject("CityDataUI").AddComponent<CityDataUI>();
            _cityDataUI.Initialize(_clientRepository);

            
            MslLogger.LogSuccess($"Mod enable. Server active : {_isServerEnabled}");
        }

        public void OnLevelUnloading()
        {
            if (_cityDataUI != null)
            {
                GameObject.Destroy(_cityDataUI);
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