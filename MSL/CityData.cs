using System;

namespace MSL
{
    [Serializable]
    public class CityData
    {
        public string CityName { get; set; }
        public int ElectricConsumption { get; set; }
        public int ElectricProduction { get; set; }
        public int ElectricExtra { get; set; }
    }
}