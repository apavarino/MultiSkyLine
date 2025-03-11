using System.Collections.Generic;
using System.Linq;
using MSL.model;

namespace MSL.server.db
{
    public class CityDataInMemoryRepository
    {
        private readonly Dictionary<string, CityData> _citiesData = new Dictionary<string, CityData>();
        
        public void UpdatePlayerCityData(string player, CityData cityData)
        {
            _citiesData[player] = cityData;
        }
        
        public Dictionary<string, CityData> FetchAllPlayersCityData()
        {
            return _citiesData;
        }

        public List<Contract> FetchAllOpenContracts()
        {
           return _citiesData
               .SelectMany(city => city.Value.Contracts
                    .Where(contract => !contract.Active))
               .ToList();
        }
    }
}