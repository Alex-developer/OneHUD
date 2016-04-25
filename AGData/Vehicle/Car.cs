using System;
using System.Linq;

namespace OneHUDData.Vehicle
{
    public class Car
    {
        // True if the player is in the car False if not
        public bool InCar { get; set; }

        // Speed of the car in MPH
        public float Speed { get; set; }

        // Gear -1 = R, 0 = N, 1 =1 etc
        public int Gear { get; set; }

        // Litres of fuel remaining
        public float FuelRemaining { get; set; }

        // Fuel tank capacity
        public float FuelCapacity { get; set; }
    
    }
}
