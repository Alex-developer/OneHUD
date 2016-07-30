using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RaceRoom.DataFormat
{

    public class GameData
    {
        [JsonProperty("cars")]
        public Car[] Cars { get; set; }

        [JsonProperty("liveries")]
        public Livery[] Liveries { get; set; }

        [JsonProperty("tracks")]
        public Track[] Tracks { get; set; }

        [JsonProperty("layouts")]
        public Layout[] Layouts { get; set; }

        [JsonProperty("classes")]
        public Class[] Classes { get; set; }

        [JsonProperty("manufacturers")]
        public Manufacturer[] Manufacturers { get; set; }

        [JsonProperty("teams")]
        public Team[] Teams { get; set; }
    }

    public class Driver
    {
        [JsonProperty("Forename")]
        public string Forename { get; set; }

        [JsonProperty("Surname")]
        public string Surname { get; set; }

        [JsonProperty("Id")]
        public int Id { get; set; }
    }

    public class Livery
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Team")]
        public int Team { get; set; }

        [JsonProperty("Car")]
        public int Car { get; set; }

        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonProperty("Class")]
        public int Class { get; set; }

        [JsonProperty("TeamName")]
        public string TeamName { get; set; }

        [JsonProperty("drivers")]
        public Driver[] Drivers { get; set; }
    }

    public class Car
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("BrandName")]
        public string BrandName { get; set; }

        [JsonProperty("CarManufacturer")]
        public int CarManufacturer { get; set; }

        [JsonProperty("DefaultLivery")]
        public int DefaultLivery { get; set; }

        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonProperty("Class")]
        public int Class { get; set; }

        [JsonProperty("liveries")]
        public Livery[] Liveries { get; set; }
    }

    public class Layout
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Track")]
        public int Track { get; set; }

        [JsonProperty("MaxNumberOfVehicles")]
        public int MaxNumberOfVehicles { get; set; }

        [JsonProperty("Id")]
        public int Id { get; set; }
    }

    public class Track
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonProperty("layouts")]
        public Layout[] Layouts { get; set; }
    }

    public class CarsInClass
    {
        [JsonProperty("Id")]
        public int Id { get; set; }
    }

    public class Class
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Cars")]
        public CarsInClass[] CarsInClass { get; set; }

        [JsonProperty("Id")]
        public int Id { get; set; }
    }

    public class Manufacturer
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Id")]
        public int Id { get; set; }
    }

    public class Team
    {
        [JsonProperty("Type")]
        public string Type { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Id")]
        public int Id { get; set; }
    }

}
