using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StationWeb.DTOs;

namespace StationWeb.DTOs
{
    public class StationPublicDetails
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Double LocLongitude { get; set; }
        public Double LocLattitude { get; set; }
        public List<SensorDetails> Sensors { get; set; }
    }
}