using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MotorGliding.Models.Other
{
    [NotMapped]
    public class WeatherInfo
    {
        //public class coord
        //{
        //    public double lon { get; set; }
        //    public double lat { get; set; }
        //}

        //public class weather
        //{
        //    public int id { get; set; }
        //    public string main { get; set; }
        //    public string description { get; set; }
        //    public string icon { get; set; }
        //}

        //public class main
        //{
        //    public double temp { get; set; }
        //    public double pressure { get; set; }
        //    public double humidity { get; set; }

        //}

        //public class wind
        //{
        //    public double speed { get; set; }

        //}

        //public class sys
        //{
        //    public string country { get; set; }

        //}
        //public class Root
        //{
        //    public string name { get; set; }
        //    public sys sys { get; set; }
        //    public double dt { get; set; }
        //    public wind wind { get; set; }
        //    public main main { get; set; }
        //    public weather weather { get; set; }
        //    public List<weather> weatherList { get; set; }
        //    public coord coordinate { get; set; }
        //}


        public class Root
        {
            public Coord coord { get; set; }
            public Weather[] weather { get; set; }
            public string _base { get; set; }
            public Main main { get; set; }
            public int visibility { get; set; }
            public Wind wind { get; set; }
            public Clouds clouds { get; set; }
            public int dt { get; set; }
            public Sys sys { get; set; }
            public int id { get; set; }
            public string name { get; set; }
            public int cod { get; set; }
        }

        public class Coord
        {
            public float lon { get; set; }
            public float lat { get; set; }
        }

        public class Main
        {
            public float temp { get; set; }
            public int pressure { get; set; }
            public int humidity { get; set; }
            public float temp_min { get; set; }
            public float temp_max { get; set; }
        }

        public class Wind
        {
            public float speed { get; set; }
            public int deg { get; set; }
        }

        public class Clouds
        {
            public int all { get; set; }
        }

        public class Sys
        {
            public int type { get; set; }
            public int id { get; set; }
            public float message { get; set; }
            public string country { get; set; }
            public int sunrise { get; set; }
            public int sunset { get; set; }
        }

        public class Weather
        {
            public int id { get; set; }
            public string main { get; set; }
            public string description { get; set; }
            public string icon { get; set; }
        }

    }
}
