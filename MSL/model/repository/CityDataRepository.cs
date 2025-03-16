using System.Collections.Generic;
using System.Linq;

namespace MSL.model.repository
{
    /// <summary>
    /// Repository for managing city data and associated contracts.
    /// </summary>
    public class CityDataRepository
    {
        private Dictionary<string, CityData> _citiesData = new Dictionary<string, CityData>();
        private readonly string _currentCity;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="CityDataRepository"/> class.
        /// </summary>
        /// <param name="currentCity">The name of the current city.</param>
        public CityDataRepository(string currentCity)
        {
            _currentCity = currentCity;
        }
        
        /// <summary>
        /// Gets the name of the current city. Null if server
        /// </summary>
        /// <returns>The name of the current city.</returns>
        public string FindCurrentCityName()
        {
            return _currentCity;
        }
        
        /// <summary>
        /// Updates the data for the current city.
        /// </summary>
        /// <param name="cityData">The new city data.</param>
        public void UpdateOne(CityData cityData)
        {
            _citiesData[_currentCity] = cityData;
        }

        /// <summary>
        /// Updates the data for a specified city.
        /// </summary>
        /// <param name="cityName">The name of the city.</param>
        /// <param name="cityData">The new city data.</param>
        public void UpdateOneByCityName(string cityName, CityData cityData)
        {
            _citiesData[cityName] = cityData;
        }
        
        /// <summary>
        /// Updates the data for all cities.
        /// </summary>
        /// <param name="citiesData">A dictionary containing city names and their corresponding data.</param>
        public void UpdateAll(Dictionary<string, CityData> citiesData)
        {
            _citiesData = citiesData;
        }
        
        /// <summary>
        /// Retrieves all stored city data.
        /// </summary>
        /// <returns>A dictionary containing city names and their corresponding data.</returns>
        public Dictionary<string, CityData> FindAll()
        {
            return _citiesData;
        }
        
        /// <summary>
        /// Retrieves the data for the current city, creating an entry if it does not exist.
        /// </summary>
        /// <returns>The city data for the current city.</returns>
        public CityData FindCurrentCityData()
        {
            CreateIfNotExists(_currentCity);
            return _citiesData[_currentCity];
        }
        
        /// <summary>
        /// Retrieves the contracts associated with the current city.
        /// </summary>
        /// <returns>A list of contracts for the current city, or null if the city is not found.</returns>
        public List<Contract> FindContracts()
        {
            return !_citiesData.TryGetValue(_currentCity, out var value) ? null : value.Contracts;
        }
        
        /// <summary>
        /// Retrieves all open (inactive) contracts across all cities.
        /// </summary>
        /// <returns>A list of all open contracts.</returns>
        public List<Contract> FindAllOpenContracts()
        {
           return _citiesData
               .SelectMany(city => city.Value.Contracts
                    .Where(contract => !contract.Active))
               .ToList();
        }

        /// <summary>
        /// Adds a contract to the current city's data, creating an entry if necessary.
        /// </summary>
        /// <param name="contract">The contract to add.</param>
        public void AddContract(Contract contract)
        {
            CreateIfNotExists(_currentCity);
            _citiesData[_currentCity].Contracts.Add(contract);
        }

        /// <summary>
        /// Ensures that a city's data entry exists; if not, creates a new entry.
        /// </summary>
        /// <param name="cityName">The name of the city.</param>
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
