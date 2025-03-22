using ColossalFramework.UI;
using ICities;
using MSL.client.controller;
using MSL.client.ui;
using MSL.model.repository;
using MSL.model;
using MSL.server;
using MSL.utils;
using UnityEngine;

namespace MSL
{
    public class Msl : IUserMod, ILoadingExtension
    {
        public string Name => "MSL";
        public string Description => "MultiSkyLine";

        private EmbeddedServer _server;
        
        private CityDataConnector _cityDataConnector;
        private CityDataRepository _clientRepository;

        private const string DefaultServerIP = "127.0.0.1";
        public static string ServerIP = DefaultServerIP;
        private static bool _isServerEnabled = true;

        private static MslConfig _config = new MslConfig();

        private CityDataUI _cityDataUI;
        
        public void OnEnabled()
        {
            _config = Configs.LoadConfig();
            _isServerEnabled = _config.IsServerEnabled;
            if (!_isServerEnabled)
            {
                ServerIP = _config.ServerURL;
            }
        }

        public void OnDisabled()
        {
            Configs.SaveConfig(_config);
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
                ServerIP = _config.ServerURL;
            }
            
            _clientRepository = new CityDataRepository(SimulationManager.instance.m_metaData.m_CityName);
            _cityDataConnector = new CityDataConnector(_clientRepository);
            _cityDataConnector.Start();
            
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
            
            _cityDataConnector?.Stop();
            StopServer();
            Configs.SaveConfig(_config);
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
                _config.IsServerEnabled = _isServerEnabled;
                if (_isServerEnabled)
                {
                    MslLogger.LogServer("Updating the server ip to local url");
                    ServerIP = DefaultServerIP;
                    StartServer();
                }
                else
                {
                    MslLogger.LogServer("Updating the server ip to distant url");
                    ServerIP = _config.ServerURL;
                    StopServer();
                }

                if (textfield != null)
                {
                    textfield.text = ServerIP;
                }
            });

            textfield = group.AddTextfield("Server address", ServerIP, (value) =>
            {
                ServerIP = value;

                if (!_isServerEnabled)
                {
                    _config.ServerURL = ServerIP;
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