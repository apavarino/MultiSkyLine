using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using fastJSON;
using MSL.client.ui;
using MSL.model;
using MSL.model.repository;
using UnityEngine;

namespace MSL.client.controller
{
    public class CityDataFetcher
    {
        private static readonly WebClient Client = new WebClient();
        private readonly string _serverUrl = $"http://{Msl.ServerIP}:5000/api/cityData/all";
        private Timer _timer;
        private readonly CityDataRepository _cityDataRepository;
        
        public CityDataFetcher(CityDataRepository cityDataRepository )
        {
            _cityDataRepository = cityDataRepository;
        }
        
        public void Start()
        {
            MslLogger.LogStart("Starting city data fetcher");
            _timer = new Timer(FetchCityData, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
        }

        public void Stop()
        {
            MslLogger.LogError("Stopping city data fetcher");
            _timer?.Dispose();
        }

        private void FetchCityData(object state)
        {
            MslLogger.LogSend($"Requesting city data ({_serverUrl})");

            // Avoid multiple subscribe
            Client.DownloadStringCompleted -= OnDownloadStringCompleted;
            Client.DownloadStringCompleted += OnDownloadStringCompleted;

            Client.DownloadStringAsync(new Uri(_serverUrl));
        }

        private void OnDownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MslLogger.LogError($"Error fetching city data: {e.Error.Message}");
                return;
            }

            try
            {
                if (e.Result != null && e.Result.Trim().Length > 0 && e.Result.Trim() != "{}")
                {
                    _cityDataRepository.UpdateAll(JSON.ToObject<Dictionary<string, CityData>>(e.Result));

                    var ui = GameObject.FindObjectOfType<CityDataUI>();
                    if (ui != null)
                    {
                        ui.UpdateCityDataDisplay();
                    }
                }

                MslLogger.LogSuccess("City data successfully updated!");
            }
            catch (Exception ex)
            {
                MslLogger.LogError($"Error parsing city data: {ex.Message}");
            }
        }
    }
}
