using System;
using System.Net;
using ColossalFramework;
using fastJSON;
using MSL.model;
using MSL.model.repository;

namespace MSL.client.controller
{
    public class CityDataEmitter
    {
        private readonly string _serverUrl = $"http://{Msl.ServerIP}:5000/api/cityData/update";
        private readonly JSONParameters _jsonParams = new JSONParameters
        {
            UseExtensions = false,
            UseFastGuid = false,
            SerializeNullValues = false,
            ShowReadOnlyProperties = true
        };
        private readonly CityDataRepository _cityDataRepository;
        
        public CityDataEmitter(CityDataRepository cityDataRepository )
        {
            _cityDataRepository = cityDataRepository;
        }
        
        public void SendCityData()
        {
            try
            {
                var districtManager = Singleton<DistrictManager>.instance;
                var production = districtManager.m_districts.m_buffer[0].GetElectricityCapacity();
                var consumption = districtManager.m_districts.m_buffer[0].GetElectricityConsumption();
                var extra = production - consumption;

                
                var payload = new CityData
                {
                    CityName = _cityDataRepository.FindCurrentCityName(),
                    ElectricConsumption = consumption,
                    ElectricProduction = production,
                    ElectricExtra = extra,
                    Contracts = _cityDataRepository.FindContracts()
                };

                var json = JSON.ToJSON(payload, _jsonParams);
                MslLogger.LogSend($"JSON generated : {json}");

                using (var client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    MslLogger.LogSend($"Sending to {_serverUrl}...");
                    client.UploadStringAsync(new Uri(_serverUrl), "POST", json);
                    _cityDataRepository.UpdateOne(payload);
                }
            }
            catch (Exception ex)
            {
                MslLogger.LogError($"Error sending request : {ex.Message}");
            }
        }
    }
}