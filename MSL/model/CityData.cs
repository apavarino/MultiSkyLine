using System;
using System.Collections.Generic;

namespace MSL.model
{
    [Serializable]
    public class CityData
    {
        public string CityName { get; set; }
        public int ElectricConsumption { get; set; }
        public int ElectricProduction { get; set; }
        public int ElectricExtra { get; set; }
        public List<Contract> Contracts { get; set; }
    }
}