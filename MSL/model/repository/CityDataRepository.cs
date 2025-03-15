using System.Collections.Generic;
using System.Linq;

namespace MSL.model.repository
{
    public class CityDataRepository
    {
        private Dictionary<string, CityData> _citiesData = new Dictionary<string, CityData>();
        private readonly string _currentCity;
        
        public CityDataRepository(string currentCity)
        {
            _currentCity = currentCity;
        }
        
        public string FindCurrentCityName()
        {
            return _currentCity;
        }
        
        public void UpdateOne( CityData cityData)
        {
            _citiesData[_currentCity] = cityData;
        }

        public void UpdateOneByCityName(string cityName, CityData cityData)
        {
            _citiesData[cityName] = cityData;
        }
        
        public void UpdateAll(Dictionary<string, CityData> citiesData)
        {
            _citiesData = citiesData;
        }
        
        public Dictionary<string, CityData> FindAll()
        {
            return _citiesData;
        }
        
        public CityData FindCurrentCityData()
        {
            CreateIfNotExists(_currentCity);
            return _citiesData[_currentCity];
        }
        
        public List<Contract> FindContracts()
        {
            return !_citiesData.TryGetValue(_currentCity, out var value) ? null : value.Contracts;
        }
        
        public List<Contract> FindAllOpenContracts()
        {
           return _citiesData
               .SelectMany(city => city.Value.Contracts
                    .Where(contract => !contract.Active))
               .ToList();
        }

        public void AddContract(Contract contract)
        {
            CreateIfNotExists(_currentCity);
            _citiesData[_currentCity].Contracts.Add(contract);
        }

        private void CreateIfNotExists(string cityName)
        {
            if (!_citiesData.ContainsKey(cityName))
            {
                _citiesData[cityName] = new CityData
                {
                    CityName = cityName
                };
            }
        }
    }
}