using System;
using System.Net;
using System.Threading;
using ColossalFramework;
using fastJSON;
using MSL.model;

namespace MSL
{
    public class CityDataEmitter
    {
        private readonly string _serverUrl = $"http://{Msl.ServerIP}:5000/api/cityData/update";
        private readonly string _cityName = SimulationManager.instance.m_metaData.m_CityName;
        private readonly JSONParameters _jsonParams = new JSONParameters
        {
            UseExtensions = false,
            UseFastGuid = false,
            SerializeNullValues = false,
            ShowReadOnlyProperties = true
        };
        private Timer _timer;

        public void Start()
        {
            MslLogger.LogStart("Starting city data emitter");
            _timer = new Timer(SendCityData, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
        }

        public void Stop()
        {
            MslLogger.LogStop("Stopping city data emitter");
            _timer?.Dispose();
        }

        private void SendCityData(object state)
        {
            try
            {
                var districtManager = Singleton<DistrictManager>.instance;
                var production = districtManager.m_districts.m_buffer[0].GetElectricityCapacity();
                var consumption = districtManager.m_districts.m_buffer[0].GetElectricityConsumption();
                var extra = production - consumption;

                
                var payload = new CityData
                {
                    CityName = _cityName,
                    ElectricConsumption = consumption,
                    ElectricProduction = production,
                    ElectricExtra = extra
                };

                var json = JSON.ToJSON(payload, _jsonParams);
                MslLogger.LogSend($"JSON generated : {json}");

                using (var client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    MslLogger.LogSend($"Sending to {_serverUrl}...");
                    client.UploadStringAsync(new Uri(_serverUrl), "POST", json);
                }
            }
            catch (Exception ex)
            {
                MslLogger.LogError($"Error sending request : {ex.Message}");
            }
        }
    }
}