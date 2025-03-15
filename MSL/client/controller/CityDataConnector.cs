using System;
using System.Threading;
using MSL.model.repository;

namespace MSL.client.controller
{
    public class CityDataConnector
    {
        private readonly CityDataEmitter _cityDataEmitter;
        private readonly CityDataFetcher _cityDataFetcher;
        private Timer _timer;
        
        public CityDataConnector(
            CityDataRepository cityDataRepository)
        {
            _cityDataEmitter = new CityDataEmitter(cityDataRepository);
            _cityDataFetcher = new CityDataFetcher(cityDataRepository);
        }

        public void Start()
        {
            MslLogger.LogStart("Starting city data connector");
            _timer = new Timer(Handle, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        }

        public void Stop()
        {
            MslLogger.LogStop("Stopping city data connector");
            _timer?.Dispose();
        }

        private void Handle(object state)
        {
            _cityDataEmitter.SendCityData(_cityDataFetcher.FetchCityData);
        }
    }
}